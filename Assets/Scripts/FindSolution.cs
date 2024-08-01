using UnityEngine;
using Zenject;

public class FindSolution : MonoBehaviour
{
    [Inject] private readonly JSONReaderService jsonReaderService;

    [SerializeField] private string modelsPath = "model";
    [SerializeField] private string spacePath = "space";

    private void Start()
    {
        Solve();
    }

    private void Solve()
    {
        var modelText = jsonReaderService.ReadJSON(modelsPath);
        var spaceText = jsonReaderService.ReadJSON(spacePath);
        Debug.Log(modelText);
        Debug.Log(spaceText);
    }
}