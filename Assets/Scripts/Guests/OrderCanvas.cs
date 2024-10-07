using System.Collections.Generic;
using UnityEngine;

public class OrderCanvas : MonoBehaviour {
	public DishText dishTextPrefab;
	List<DishText> dishTexts = new List<DishText>();
	[SerializeField]
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
}