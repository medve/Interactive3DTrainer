using UnityEngine;
using System.Collections;

public class KinectFigures 
{
//атомарные действия:
	//правая/левая рука сжата в кулак
	//правая и левая руки сведены/разведены
	//руки сходятся/расходятся
	//относительное расстояние между руками не изменяется
	//правая левая рука вращается
//вектор изменения позиции рук относительно тела(переход на 
	//другое место не должен считаться за движение рук)
//предыдущая позиция
//предыдущее состояние
//конечный автомат для распознавания жестов


	public bool HandsDragTowards()
	{
		//заглушка
		return false;	
	}

	public bool LeftHandDrag()
	{
		//заглушка
		return false;
	}

	public bool RightHandDrag()
	{
		//заглушка
		return false;
	}

	public bool HandsClap()
	{
		//заглушка
		return false;
	}

	public bool RightHandDoubleShrink()
	{
		//заглушка
		return false;
	}

	public bool LeftHandDoubleShrink()
	{
		//заглушка
		return false;
	}

	public bool RightHandRound()
	{
		//заглушка
		return false;
	}

	public Vector3 RightHandMoveVector()
	{
		//заглушка
		return Vector3.zero;
	}

	public Vector3 LeftHandMoveVector()
	{
		//заглушка
		return Vector3.zero;		
	}

	public Vector3 TowardMoveVector()
	{
		//заглушка
		return Vector3.zero;		
	}

	// private bool RecognizeFigure()
	// {
	// 	//получает комплексный параметр и решает имеет ли фигура место
	// 	//возможно автомат сам должен это делать на каждом шаге и эта функция не имеет смысла
	// }
}
