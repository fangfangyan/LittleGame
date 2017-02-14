using UnityEngine;
using System;
using System.Collections;

public class GamePlayException : Exception {
	
	public GamePlayException(){

	}

	public GamePlayException(String discription):base(discription){
		
	}

	///<summary>
	///Object types in the game
	///Initial With R means it’s Resources
	///Initial With G means it’s GameObject or it’s property
	///Initial With C means it’s Customized defined
	///</summary>
	public enum ObjectType
	{
		RPrefab, GObject, GConponent, CSprite, CScript
	}

	public static void ThrowExceptionNotFound(ObjectType type)
	{
		throw new NotFoundException (type.ToString ());
	}

	public static void ThrowExceptionNotFound(ObjectType type, string name)
	{
		throw new NotFoundException (type.ToString () + "." + name);
	}
}
