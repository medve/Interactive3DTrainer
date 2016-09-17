public class Lock
{
	//TODO: сделать через встроенные семафоры или через что-нибудь еще, понадежнее
	private int _lockState = 0;

	public bool LockUp(int lockForce = 1)
	{
		if(_lockState > 0){
			return false;
		} else {
			_lockState += lockForce;
			return true;
		}
	}

	public void Down()
	{
		_lockState -= 1;
	}
}