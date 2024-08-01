using UnityEngine;
using Zenject;

public class FindSolution : MonoBehaviour
{
    [Inject] private readonly JSONReaderService jsonReaderService;
    [Inject] private readonly JSONMatrixFillService matrixFillService;
    [Inject] private readonly ConfigProvider configProvider;

    private void Start()
    {
        Solve();
    }

    private void Solve()
    {
        var modelText = jsonReaderService.ReadJSON(configProvider.ModelsPath);
        var spaceText = jsonReaderService.ReadJSON(configProvider.SpacePath);

        var modelData = matrixFillService.GetMatricesFromJSON(modelText);
        var spaceData = matrixFillService.GetMatricesFromJSON(spaceText);

        modelData.VisualizeOnSceneAsModel(configProvider);
        spaceData.VisualizeOnSceneAsSpace(configProvider);
    }
}