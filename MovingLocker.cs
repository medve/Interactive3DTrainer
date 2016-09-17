using UnityEngine;
using System.Collections;

public class MovingLocker : MonoBehaviour
{
	private Lock _movingLock;

	private void Start()
	{
		_movingLock = new Lock();
	}

	public bool LockUp(int lockForce = 1)
	{
		return _movingLock.LockUp(lockForce);
	}
	
	public void Down()
	{
		_movingLock.Down();
	}

}