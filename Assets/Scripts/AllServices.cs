using DefaultNamespace;
using Services;
using Ui.Factory;
using Ui.Services;
using UnityEngine;

public sealed class AllServices : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private UpdateService updateService;
    private AssetProvider _assets;
    private IGameFactory _factory;

    private IInputService _inputService;
    private IRandomService _random;
    private IRaycastService _raycastService;
    private UiFactory _uiFactory;
    private WindowEditingService _windowEditingService;
    private WindowPlacingService _windowPlacingService;
    private IWindowSelectionService _windowSelectionService;
    private WindowSelectionVisualService _windowSelectionVisualService;
    private WindowService _windowService;
    private IStaticDataService _staticData;

    private void Awake()
    {
        _staticData = new StaticDataService();
        _inputService = new InputService();
        _assets = new AssetProvider();
        _random = new RandomService();
        _raycastService = new RaycastService();

        _uiFactory = new UiFactory(_assets);
        _windowService = new WindowService(_uiFactory);
        _factory = new GameFactory(_assets, _windowService, _uiFactory, _random, _raycastService, _inputService, _staticData);


        _windowSelectionVisualService = new WindowSelectionVisualService(_inputService, rectTransform);
        _windowSelectionService = new WindowSelectionService(_inputService, _factory, _windowSelectionVisualService,
            _raycastService);
        _windowPlacingService = new WindowPlacingService(_inputService, _factory, _raycastService);
        _windowEditingService = new WindowEditingService(_inputService, _windowPlacingService, _windowSelectionService,
            _windowService, _raycastService);

        _uiFactory.InjectWindowServices(_windowPlacingService, _windowEditingService);
    }

    private void Start()
    {
        updateService.Construct(_inputService);

        _staticData.Load();

        _uiFactory.CreateUiRoot();
        _factory.CreateHud();
        _factory.CreateCamera();
    }
}