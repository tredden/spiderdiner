using System.Collections.Generic;
using UnityEngine;

public class OrderCanvas : MonoBehaviour {
	
	public DishText dishTextPrefab;
	[SerializeField]
	List<DishText> dishTexts = new List<DishText>();
	List<Dish> dishes = new List<Dish>();

	[SerializeField]
	UnityEngine.UI.Image satisfactionBar;
	[SerializeField]
	float maxBarWidth;

	public void UpdateOrder(GuestOrder order) {
		// Only show dishes that are not eaten.
		dishes = order.dishes.FindAll(d => !order.eatenDishes.Contains(d));
		if (dishes.Count < dishTexts.Count) {
			for (int i = dishes.Count; i < dishTexts.Count; i++) {
				dishTexts[i].gameObject.SetActive(false);
				GameObject.Destroy(dishTexts[i].gameObject);
			}
			dishTexts.RemoveRange(dishes.Count, dishTexts.Count - dishes.Count);
		} else if (dishes.Count > dishTexts.Count) {
			for (int i = dishTexts.Count; i < dishes.Count; i++) {
				DishText newText = Instantiate<DishText>(dishTextPrefab, this.transform);
				newText.transform.localPosition = new Vector3(0, 2 * i, 0);
				dishTexts.Add(newText);
			}
		}

		// Set the height of this canvas to fit all the dishes
		RectTransform rt = this.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(rt.sizeDelta.x, 2.8f * dishes.Count);

		for (int i = 0; i < dishes.Count; i++) {
			dishTexts[i].SetDish(dishes[i]);
		}
	}

	public void UpdateSatisfaction(float current, float max)
    {
		Vector2 size = satisfactionBar.rectTransform.sizeDelta;
		float frac = current / max;
		size.x = frac * maxBarWidth;
		satisfactionBar.rectTransform.sizeDelta = size;
		satisfactionBar.color = frac < .5f ? Color.Lerp(Color.red, Color.yellow, frac * 2f) : Color.Lerp(Color.yellow, Color.green, frac * 2f - 1f);
	}
}