using System.Collections.Generic;
using UnityEngine;

public class OrderCanvas : MonoBehaviour {
	public DishText dishTextPrefab;
	List<DishText> dishTexts = new List<DishText>();
	[SerializeField]
	RectTransform Bubble;

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

		for (int i = 0; i < order.dishes.Count; i++) {
			dishTexts[i].SetDish(order.dishes[i]);
		}
	}
}