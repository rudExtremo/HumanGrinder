using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HumanGrinder
{
    // A deliberately compact, asset-free vertical slice for the game jam.
    // Replace with testable systems and authored assets as the project evolves.
    public sealed class HumanGrinderPrototype : MonoBehaviour
    {
        private enum Phase { Menu, Fighting, Ascending, Victory, Defeat }
        private sealed class Fighter
        {
            public GameObject View;
            public float X;
            public float Y = FloorY;
            public float VelocityY;
            public int Facing = 1;
            public int HP;
            public int MaxHP;
            public int Damage;
            public float Speed;
            public float AttackCooldown;
            public bool Alive => HP > 0;
        }

        private sealed class Enemy
        {
            public GameObject View;
            public float X;
            public int HP;
            public float AttackCooldown;
        }

        private static readonly string[] CharacterNames = { "Vanguard", "Duelist", "Crusher" };
        private static readonly int[] TowerFloors = { 5, 10, 15 }; // Provisional balance values.
        private const float FloorY = -2.15f;
        private const float Gravity = -22f;
        private const float JumpVelocity = 8f;

        private readonly List<Enemy> enemies = new List<Enemy>();
        private readonly Fighter[] fighters = new Fighter[2];
        private Sprite square;
        private GameObject elevator;
        private Phase phase = Phase.Menu;
        private int selectedP1;
        private int selectedP2 = 1;
        private int selectedTower = 1;
        private int floor;
        private int enemiesPending;
        private float spawnTimer;
        private float ascentTimer;
        private bool joinP2AtStart;
        private float towerScroll;

        private void Awake()
        {
            square = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1),
                new Vector2(0.5f, 0.5f), 1f);
            if (Camera.main != null)
            {
                Camera.main.orthographic = true;
                Camera.main.orthographicSize = 5.6f;
            }
            elevator = Block("Elevator platform", 16f, 0.25f, new Color(0.38f, 0.40f, 0.46f));
            elevator.transform.position = new Vector3(0f, FloorY - 0.55f, 0f);
        }

        private static Keyboard Keys => Keyboard.current;

        private static bool Pressed(UnityEngine.InputSystem.Controls.KeyControl key)
        {
            return key != null && key.wasPressedThisFrame;
        }

        private static bool Held(UnityEngine.InputSystem.Controls.KeyControl key)
        {
            return key != null && key.isPressed;
        }

        private static bool P2Input(Keyboard keyboard)
        {
            return Pressed(keyboard.leftArrowKey) || Pressed(keyboard.rightArrowKey) ||
                Pressed(keyboard.upArrowKey) || Pressed(keyboard.periodKey) ||
                Pressed(keyboard.slashKey);
        }

        private void Update()
        {
            Keyboard keyboard = Keys;
            if (keyboard == null) return;

            if (phase == Phase.Menu)
            {
                if (P2Input(keyboard)) joinP2AtStart = true;
                return;
            }
            if (phase == Phase.Defeat || phase == Phase.Victory)
            {
                if (Pressed(keyboard.enterKey)) ReturnToMenu();
                return;
            }

            if (fighters[1] == null && P2Input(keyboard))
            {
                fighters[1] = CreateFighter(1, selectedP2);
            }

            float dt = Mathf.Min(Time.deltaTime, 0.05f);
            UpdateFighter(fighters[0], dt, keyboard.aKey, keyboard.dKey,
                keyboard.spaceKey, keyboard.fKey);
            UpdateFighter(fighters[1], dt, keyboard.leftArrowKey, keyboard.rightArrowKey,
                keyboard.upArrowKey, keyboard.periodKey, keyboard.slashKey);

            if (phase == Phase.Ascending)
            {
                ascentTimer -= dt;
                towerScroll += dt * 2f;
                if (ascentTimer <= 0f)
                {
                    floor++;
                    BeginFloor();
                }
                return;
            }

            UpdateEnemies(dt);
            spawnTimer -= dt;
            if (enemiesPending > 0 && spawnTimer <= 0f)
            {
                SpawnEnemy();
                enemiesPending--;
                spawnTimer = Mathf.Max(0.30f, 1.0f - 0.03f * floor);
            }

            if (AllPlayersDead())
            {
                phase = Phase.Defeat;
            }
            else if (enemiesPending == 0 && enemies.Count == 0)
            {
                if (floor >= TowerFloors[selectedTower])
                {
                    phase = Phase.Victory;
                }
                else
                {
                    phase = Phase.Ascending;
                    ascentTimer = 1.8f;
                }
            }
        }

        private void UpdateFighter(Fighter fighter, float dt,
            UnityEngine.InputSystem.Controls.KeyControl left,
            UnityEngine.InputSystem.Controls.KeyControl right,
            UnityEngine.InputSystem.Controls.KeyControl jump,
            UnityEngine.InputSystem.Controls.KeyControl attack,
            UnityEngine.InputSystem.Controls.KeyControl alternateJump = null)
        {
            if (fighter == null || !fighter.Alive) return;
            float horizontal = (Held(right) ? 1f : 0f) - (Held(left) ? 1f : 0f);
            if (horizontal != 0f) fighter.Facing = horizontal > 0f ? 1 : -1;
            fighter.X = Mathf.Clamp(fighter.X + horizontal * fighter.Speed * dt, -7.9f, 7.9f);
            bool grounded = fighter.Y <= FloorY + 0.001f;
            if (grounded && (Pressed(jump) || Pressed(alternateJump)))
            {
                fighter.VelocityY = JumpVelocity;
            }
            fighter.VelocityY += Gravity * dt;
            fighter.Y += fighter.VelocityY * dt;
            if (fighter.Y < FloorY)
            {
                fighter.Y = FloorY;
                fighter.VelocityY = 0f;
            }
            fighter.AttackCooldown = Mathf.Max(0f, fighter.AttackCooldown - dt);
            if (Pressed(attack) && fighter.AttackCooldown <= 0f && phase == Phase.Fighting)
            {
                Slash(fighter);
                fighter.AttackCooldown = 0.35f;
            }
            fighter.View.transform.position = new Vector3(fighter.X, fighter.Y, 0f);
        }

        private void Slash(Fighter fighter)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = enemies[i];
                float dx = enemy.X - fighter.X;
                if (Mathf.Abs(dx) > 1.7f || Mathf.Sign(dx) != fighter.Facing ||
                    Mathf.Abs(fighter.Y - FloorY) > 1.5f)
                {
                    continue;
                }
                enemy.HP -= fighter.Damage;
                enemy.X = Mathf.Clamp(enemy.X + fighter.Facing * 0.55f, -8f, 8f);
                if (enemy.HP <= 0)
                {
                    Destroy(enemy.View);
                    enemies.RemoveAt(i);
                }
            }
        }

        private void UpdateEnemies(float dt)
        {
            foreach (Enemy enemy in enemies)
            {
                Fighter target = NearestTarget(enemy.X);
                if (target == null) break;
                float dx = target.X - enemy.X;
                if (Mathf.Abs(dx) > 0.9f)
                {
                    enemy.X = Mathf.MoveTowards(enemy.X, target.X, dt * (1.5f + 0.035f * floor));
                }
                enemy.AttackCooldown -= dt;
                if (Mathf.Abs(dx) <= 0.9f && target.Y < FloorY + 1.0f &&
                    enemy.AttackCooldown <= 0f)
                {
                    target.HP = Mathf.Max(0, target.HP - 9);
                    enemy.AttackCooldown = 1.1f;
                    if (!target.Alive)
                    {
                        target.View.GetComponent<SpriteRenderer>().color = Color.gray;
                    }
                }
                enemy.View.transform.position = new Vector3(enemy.X, FloorY, 0f);
            }
        }

        private Fighter NearestTarget(float enemyX)
        {
            Fighter best = null;
            float distance = float.MaxValue;
            foreach (Fighter fighter in fighters)
            {
                if (fighter == null || !fighter.Alive) continue;
                float candidate = Mathf.Abs(fighter.X - enemyX);
                if (candidate < distance)
                {
                    best = fighter;
                    distance = candidate;
                }
            }
            return best;
        }

        private bool AllPlayersDead()
        {
            foreach (Fighter fighter in fighters)
            {
                if (fighter != null && fighter.Alive) return false;
            }
            return true;
        }

        private void SpawnEnemy()
        {
            bool left = Random.value > 0.5f;
            Enemy enemy = new Enemy
            {
                X = left ? -8.2f : 8.2f,
                HP = 20 + floor * 4,
                AttackCooldown = Random.Range(0.4f, 0.9f),
                View = Block("Monster", 0.85f, 1.1f, new Color(0.78f, 0.28f, 0.34f))
            };
            enemy.View.transform.position = new Vector3(enemy.X, FloorY, 0f);
            enemies.Add(enemy);
        }

        private Fighter CreateFighter(int playerIndex, int type)
        {
            Fighter fighter = new Fighter
            {
                X = playerIndex == 0 ? -1.4f : 1.4f,
                MaxHP = type == 0 ? 120 : type == 1 ? 90 : 150,
                Damage = type == 0 ? 24 : type == 1 ? 20 : 34,
                Speed = type == 0 ? 4.8f : type == 1 ? 6.4f : 3.7f,
                View = Block("Player " + (playerIndex + 1), 0.8f, 1.4f,
                    playerIndex == 0 ? new Color(0.19f, 0.72f, 0.91f) :
                    new Color(0.95f, 0.76f, 0.29f))
            };
            fighter.HP = fighter.MaxHP;
            fighter.View.transform.position = new Vector3(fighter.X, FloorY, 0f);
            return fighter;
        }

        private GameObject Block(string label, float width, float height, Color color)
        {
            GameObject result = new GameObject(label);
            SpriteRenderer renderer = result.AddComponent<SpriteRenderer>();
            renderer.sprite = square;
            renderer.color = color;
            renderer.sortingOrder = 1;
            result.transform.localScale = new Vector3(width, height, 1f);
            return result;
        }

        private void StartRun()
        {
            ClearRun();
            floor = 1;
            towerScroll = 0f;
            fighters[0] = CreateFighter(0, selectedP1);
            if (joinP2AtStart) fighters[1] = CreateFighter(1, selectedP2);
            BeginFloor();
        }

        private void BeginFloor()
        {
            phase = Phase.Fighting;
            enemiesPending = 2 + floor * 2;
            spawnTimer = 0.4f;
        }

        private void ClearRun()
        {
            foreach (Enemy enemy in enemies) Destroy(enemy.View);
            enemies.Clear();
            for (int i = 0; i < fighters.Length; i++)
            {
                if (fighters[i] != null) Destroy(fighters[i].View);
                fighters[i] = null;
            }
        }

        private void ReturnToMenu()
        {
            ClearRun();
            joinP2AtStart = false;
            phase = Phase.Menu;
        }

        private void OnGUI()
        {
            float width = Mathf.Min(560f, Screen.width - 24f);
            float x = (Screen.width - width) * 0.5f;
            if (phase == Phase.Menu)
            {
                GUILayout.BeginArea(new Rect(x, 45, width, Mathf.Max(260f, Screen.height - 60f)), GUI.skin.box);
                GUILayout.Label("HUMAN GRINDER — JAM PROTOTYPE");
                GUILayout.Space(10);
                GUILayout.Label("PLAYER 1 CHARACTER");
                CharacterButtons(true);
                GUILayout.Space(8);
                GUILayout.Label("PLAYER 2 CHARACTER (optional drop-in)");
                CharacterButtons(false);
                GUILayout.Label("P2: press Arrow / Period / Slash to join: " + (joinP2AtStart ? "READY" : "OFF"));
                GUILayout.Space(8);
                GUILayout.Label("TOWER LENGTH (provisional floor counts)");
                GUILayout.BeginHorizontal();
                for (int i = 0; i < TowerFloors.Length; i++)
                {
                    if (GUILayout.Button((selectedTower == i ? "[X] " : "") + TowerFloors[i] + " floors"))
                        selectedTower = i;
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(12);
                if (GUILayout.Button("START RUN", GUILayout.Height(42))) StartRun();
                GUILayout.Label("P1: A/D move, Space jump, F slash");
                GUILayout.Label("P2: Arrows move/jump, . slash, / alternate jump");
                GUILayout.EndArea();
                return;
            }

            GUILayout.BeginArea(new Rect(12, 10, Screen.width - 24, 100));
            GUILayout.BeginHorizontal();
            GUILayout.Label("FLOOR " + floor + " / " + TowerFloors[selectedTower], GUILayout.Width(160));
            GUILayout.Label(HealthLabel(0), GUILayout.Width(195));
            GUILayout.Label(HealthLabel(1), GUILayout.Width(215));
            GUILayout.EndHorizontal();
            if (phase == Phase.Fighting)
                GUILayout.Label("MONSTERS: " + (enemiesPending + enemies.Count));
            if (phase == Phase.Ascending)
                GUILayout.Label("ELEVATOR ASCENDING...");
            GUILayout.EndArea();

            if (phase == Phase.Victory || phase == Phase.Defeat)
            {
                GUI.Box(new Rect(x, Screen.height * 0.35f, width, 120f),
                    (phase == Phase.Victory ? "TOWER CLEARED" : "RUN OVER") +
                    "\nPress Enter to return to menu");
            }
            else if (fighters[1] == null)
            {
                GUI.Label(new Rect(20, Screen.height - 42, Screen.width - 25, 30),
                    "P2: press an arrow key, . or / to JOIN");
            }
        }

        private string HealthLabel(int index)
        {
            Fighter fighter = fighters[index];
            return fighter == null ? "P" + (index + 1) + ": NOT JOINED" :
                "P" + (index + 1) + " HP: " + fighter.HP + " / " + fighter.MaxHP;
        }

        private void CharacterButtons(bool playerOne)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < CharacterNames.Length; i++)
            {
                int current = playerOne ? selectedP1 : selectedP2;
                if (GUILayout.Button((current == i ? "[X] " : "") + CharacterNames[i]))
                {
                    if (playerOne) selectedP1 = i;
                    else selectedP2 = i;
                }
            }
            GUILayout.EndHorizontal();
        }

        private void OnDestroy()
        {
            ClearRun();
            if (elevator != null) Destroy(elevator);
            if (square != null) Destroy(square);
        }
    }
}
