using UnityEngine;
using System;
using System.Collections;

public class NotFoundException : GamePlayException {

	public NotFoundException(){
		
	}
		
	public NotFoundException(String discription):base(discription){

	}

}
