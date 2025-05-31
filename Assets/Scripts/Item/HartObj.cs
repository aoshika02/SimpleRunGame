public class HartObj : ItemObj
{
    PlayerController _playerController;
    void Start()
    {
        _playerController = PlayerController.Instance;
    }
    protected override void GetItem()
    {
        _playerController.Heal();
        Deactivate();
    }
}