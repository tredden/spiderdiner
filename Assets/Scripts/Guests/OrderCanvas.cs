using System.Collections.Generic;
using UnityEngine;

public class OrderCanvas : MonoBehaviour {
	
	public DishText dishTextPrefab;
	[SerializeField]
	List<DishText> dishTexts = new List<DishText>();

	[SerializeField]
	UnityEngine.UI.Image satisfactionBar;
	[SerializeField]
	float maxBarWidth;

	public void UpdateOrder(GuestOrder order) {
 
		if (order.dishes.Count < dishTexts.Count) {
			for (int i = order.dishes.Count; i < dishTexts.Count; i++) {
				Destroy(dishTexts[i].gameObject);
			}
			dishTexts.RemoveRange(order.dishes.Count, dishTexts.Count - order.dishes.Count);
		} else if (order.dishes.Count > dishTexts.Count) {
			for (int i = dishTexts.Count; i < order.dishes.Count; i++) {
				DishText newText = Instantiate<DishText>(dishTextPrefab, this.transform);
				newText.transform.localPosition = new Vector3(0, 2 * i, 0);
				dishTexts.Add(newText);
			}
		}

		// Set the height of this canvas to fit all the dishes
		RectTransform rt = this.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(rt.sizeDelta.x, 2.8f * order.dishes.Count);

		for (int i = 0; i < order.dishes.Count; i++) {
			dishTexts[i].SetDish(order.dishes[i]);
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