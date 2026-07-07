using UnityEngine;
using UnityEngine.UI;

public class NewUnlockedPetPanelBehavior : MonoBehaviour
{
    [SerializeField]
    int PetNumber;
    private void Start()
    {
        var horizontal = GetComponent<ScrollRect>();
    }
}