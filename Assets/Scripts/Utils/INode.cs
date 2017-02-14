using UnityEngine;
using System.Collections;

public interface INode<T> {

	T GetNext();

	void SetNext(T next);

	T GetPrev();

	void SetPrev(T prev);

}


