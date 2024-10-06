using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GuestOrder
{
	public List<Dish> dishes;
    public List<Dish> eatenDishes;
    // If true, the guest will require dishes to be served in order.
    public bool multiCourse;

    public bool CheckDone() {
        return dishes.Count == 0;
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
        for (int i = 0; i < dishes.Count; i++) {
            if (multiCourse && i > 0) {
                // Only the first dish can receive flies in multi-course mode
                return false;
            }
            if (dishes[i].ReceiveFly(fly) && dishes[i].CheckDone()) {
                eatenDishes.Add(dishes[i]);
                dishes.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    override public string ToString()
    {
        string s = "";
        foreach (Dish dish in dishes)
        {
            s += dish.ToString() + "\n";
        }

        return s.Substring(0, s.Length - 1);
    }
}

[System.Serializable]
public struct Dish
{
    public int fliesInDish;
    [HideInInspector]
    public int fliesEaten;
    public int spiceLevel;
    public FlyColor color;

    public bool CheckDone() {
        return fliesEaten == fliesInDish;
    }

    public bool ReceiveFly(Fly fly) {
        if (fly.color == color && fly.spiceLevel == spiceLevel) {
            fliesEaten++;
            return true;
        }
        return false;
    }

    public void Clear() {}

    override public string ToString() {
        return fliesEaten + " / " + fliesInDish;
    }
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

    public void RegisterGuest(Guest g)
    {
        waitingGuests.Add(g);
        g.SetStatus(GuestStatus.WAITING_FOR_TABLE);
        g.gameObject.SetActive(false);
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
                g.SetStatus(GuestStatus.WAITING_FOR_ORDER);
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
                if (g.GetStatus() == GuestStatus.WAITING_FOR_CHECK) {
                    activeGuests.Remove(g);
                    t.RemoveGuest();
                    fedGuests.Add(g);
                    g.gameObject.SetActive(false);
                } else if (g.GetStatus() == GuestStatus.EATING) {
                    g.UpdateEatingTime(dt);
                }
            }
        }
    }
}
