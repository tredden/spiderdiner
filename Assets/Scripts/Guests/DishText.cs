using UnityEngine;
public class DishText : MonoBehaviour
{
	[SerializeField]
	TMPro.TMP_Text text;

	[SerializeField]
	UnityEngine.UI.Image flyIcon;

	public void SetDish(Dish dish)
	{
		int firstText = dish.fliesEaten;
		int secondText = dish.fliesInDish;
		if (dish.fliesEaten >= dish.fliesInDish) {
			firstText = secondText;
			text.fontStyle = TMPro.FontStyles.Strikethrough & TMPro.FontStyles.Bold;
		}
		else
		{
			text.fontStyle = TMPro.FontStyles.Bold;
		}
		text.text = firstText + " / " + secondText;
		flyIcon.color = dish.targetFly.getUnityColor();
	}
}