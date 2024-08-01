using UnityEngine;

// Лучше конечно через SerializableObject, но решил побыстрее реализовать
public class ConfigProvider : MonoBehaviour
{
    [SerializeField] private string modelsPath = "model";
    [SerializeField] private string spacePath = "space";
    [SerializeField] private GameObject visualObject;
    [SerializeField] private Transform modelParentTransform;
    [SerializeField] private Transform spaceParentTransform;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    public string ModelsPath => modelsPath;
    public string SpacePath => spacePath;
    public GameObject VisualObject => visualObject;
    public Transform ModelParentTransform => modelParentTransform;
    public Transform SpaceParentTransform => spaceParentTransform;
    public Material DefaultMaterial => defaultMaterial;
    public Material SelectedMaterial => selectedMaterial;
}