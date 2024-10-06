using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GuestOrder
{
	List<Dish> dishes;
    // If true, the guest will require dishes to be served in order.
    bool multiCourse;

    public bool CheckDone() {
        return dishes.Count == 0;
    }

    public bool ReceiveFly(Fly fly) {
        for (int i = 0; i < dishes.Count; i++) {
            if (multiCourse && i > 0) {
                // Only the first dish can receive flies in multi-course mode
                return false;
            }
            if (dishes[i].ReceiveFly(fly) && dishes[i].CheckDone()) {
                dishes.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public struct Dish
{
    public int flyAmount;
    public int spiceLevel;
    public int color;

    public bool CheckDone() {
        return flyAmount == 0;
    }

    public bool ReceiveFly(Fly fly) {
        if (fly.color == color && fly.spiceLevel == spiceLevel) {
            flyAmount--;
            return true;
        }
        return false;
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
            waitingGuests.Add(t.GetGuest());
            t.RemoveGuest();
        }
    }

    public void RegisterGuest(Guest g)
    {
        waitingGuests.Add(g);
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
        t.ClearTable();
        t.RemoveGuest();
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
        timeUntilNextGuest -= dt;

        if (timeUntilNextGuest < 0f && waitingGuests.Count > 0) {
            Guest g = waitingGuests[0];
            bool success = AttemptPlaceGuest(g);
            if (success) {
                waitingGuests.Remove(g);
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
