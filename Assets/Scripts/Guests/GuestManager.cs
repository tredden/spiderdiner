using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuestOrder
{
	public List<Dish> dishes;
    public List<Dish> readyDishes;
    public List<Dish> eatenDishes;
    // If true, the guest will require dishes to be served in order.
    public bool multiCourse;
    public int activeDishIndex = 0;

    public bool CheckDone() {
        foreach (Dish d in dishes) {
            if (!d.CheckDone()) {
                return false;
            }
        }
        return true;
    }

    public void EatDish() {
        if (readyDishes.Count > 0) {
            eatenDishes.Add(readyDishes[0]);
            readyDishes.RemoveAt(0);
        }
    }

    public int fliesEaten {
		get {
			if (dishes == null) {
				return 0;
			}
			int total = 0;
			for (int i = 0; i < dishes.Count; i++) {
				total += dishes[i].fliesEaten;
			}
			return total;
		}
	}

    public bool ReceiveFly(Fly fly) {
		if (multiCourse) {
            bool ate = dishes[activeDishIndex].ReceiveFly(fly);
            if (ate) {
                if (dishes[activeDishIndex].CheckDone()) {
                    readyDishes.Add(dishes[activeDishIndex]);
                    activeDishIndex++;
                }
            }
            return ate;
        } else {
			foreach (Dish d in dishes) {
                if (d.CheckDone()) {
                    continue;
                }
                if (d.ReceiveFly(fly)) {
                    if (d.CheckDone()) {
                        readyDishes.Add(d);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}

[System.Serializable]
public class Dish
{
    public int fliesInDish;
    [HideInInspector]
    public int fliesEaten;
    [SerializeField]
    public Fly targetFly;

    public bool CheckDone() {
        return fliesEaten >= fliesInDish;
    }

    public bool ReceiveFly(Fly fly) {
        if (fly.color == targetFly.color && fly.spiceLevel == targetFly.spiceLevel) {
            fliesEaten++;
            return true;
        }
        return false;
    }

    public void Clear() {}
}

public class GuestManager : MonoBehaviour
{
    [SerializeField]
    List<Table> tables = new List<Table>();
    [SerializeField]
    List<Guest> waitingGuests = new List<Guest>();
    [SerializeField]
    List<Guest> activeGuests = new List<Guest>();
    [SerializeField]
    List<Guest> fedGuests = new List<Guest>();
    [SerializeField]
    List<Guest> angeredGuests = new List<Guest>();

    [SerializeField]
    LevelStatusUI levelStatusUI;
    [SerializeField]
    LevelCompleteWindow levelCompleteWindow;

    [SerializeField]
    float timePerGuest = 5f;
    float timeUntilNextGuest = 2f;

    static GuestManager instance;
    public static GuestManager GetInstance()
    {
        return instance;
    }

    public void RegisterTable(Table t)
    {
        if (!tables.Contains(t)) {
            tables.Add(t);
        }
    }

    public void DeregisterTable(Table t)
    {
        if (!tables.Contains(t)) {
            return;
        }
        tables.Remove(t);
        if (t.IsOccupied()) {
            Guest g = t.GetGuest();
            g.SetStatus(GuestStatus.WAITING_FOR_TABLE);
            activeGuests.Remove(g);
            waitingGuests.Add(g);
            t.RemoveGuest();
        }
    }

    void UpdateLevelStatus()
    {
        if (levelStatusUI == null) {
            levelStatusUI = GameObject.FindAnyObjectByType<LevelStatusUI>();
        }
        if (levelStatusUI != null) {
            levelStatusUI.SetMaxGuests(waitingGuests.Count + activeGuests.Count + fedGuests.Count + angeredGuests.Count);
            levelStatusUI.SetGuestsRemaining(waitingGuests.Count + activeGuests.Count);
            float maxSatisfaction = 0f;
            float currentSatisfaction = 0f;
            foreach (Guest g in waitingGuests) {
                maxSatisfaction += g.GetMaxSatisfaction();
            }
            foreach (Guest g in activeGuests) {
                maxSatisfaction += g.GetMaxSatisfaction();
            }
            foreach (Guest g in angeredGuests) {
                maxSatisfaction += g.GetMaxSatisfaction();
            }
            foreach (Guest g in fedGuests) {
                maxSatisfaction += g.GetMaxSatisfaction();
                currentSatisfaction += g.GetCurrentSatisfaction();
            }
            levelStatusUI.SetMaxSatisfaction(maxSatisfaction);
            levelStatusUI.SetCurrentSatisfaction(currentSatisfaction);
        }
    }

    public void RegisterGuest(Guest g)
    {
        waitingGuests.Add(g);
        g.SetStatus(GuestStatus.WAITING_FOR_TABLE);
        g.gameObject.SetActive(false);
        UpdateLevelStatus();
    }

    public Table FindOpenTable()
    {
        List<Table> candidates = new List<Table>();
        foreach (Table t in tables) {
            if (!t.IsOccupied()) {
                candidates.Add(t);
            }
        }
        if (candidates.Count == 0) {
            return null;
        }
        return candidates[Random.Range(0, candidates.Count)];
    }

    public bool AttemptPlaceGuest(Guest g)
    {
        Table t = FindOpenTable();
        if (t == null) {
            return false;
        }
        g.gameObject.SetActive(true);
        g.SetStatus(GuestStatus.WAITING_FOR_ORDER);
        t.ClearTable();
        t.RemoveGuest();
        t.SetGuest(g);
        return true;
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        UpdateHostStand(dt);
        UpdateActiveTables(dt);

        UpdateLevelStatus();
    }

    void UpdateHostStand(float dt)
    {
        if (FindOpenTable() != null) {
            timeUntilNextGuest -= dt;
        }

        if (timeUntilNextGuest < 0f && waitingGuests.Count > 0) {
            Guest g = waitingGuests[0];
            bool success = AttemptPlaceGuest(g);
            if (success) {
                waitingGuests.Remove(g);
                g.SetStatus(GuestStatus.PONDERING_ORDER);
                activeGuests.Add(g);
                // reset timer until guest can be seated
                timeUntilNextGuest = timePerGuest;
            }
        }
    }

    void UpdateActiveTables(float dt)
    {
        foreach (Table t in tables) {
            if (t.IsOccupied()) {
                Guest g = t.GetGuest();
                g.Tick(dt);
                if (g.GetStatus() == GuestStatus.FINISHED) {
                    activeGuests.Remove(g);
                    t.RemoveGuest();
                    if (g.GetCurrentSatisfaction() <= 0) {
                        angeredGuests.Add(g);
                    } else {
						fedGuests.Add(g);
                    }
                    g.gameObject.SetActive(false);
                    UpdateLevelStatus();
                } else if (g.GetStatus() == GuestStatus.EATING) {
                    if (waitingGuests.Count == 0 && activeGuests.Count == 0) {
						ShowDoneScreen();
                    }
                }
            }
        }
    }

    void ShowDoneScreen()
    {
        UpdateLevelStatus();
        if (levelCompleteWindow == null) {
            levelCompleteWindow = GameObject.FindAnyObjectByType<LevelCompleteWindow>(FindObjectsInactive.Include);
        }
        levelCompleteWindow.Show(levelStatusUI.GetMetTarget());
    }
}
