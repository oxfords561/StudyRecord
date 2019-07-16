// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-07-11 22:31:12
// 版 本：v 1.0
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Singleton<T>:IDisposable where T : new () {

	private static T instance;

	public static T Instance {
		get {
			if (instance == null) {
				instance = new T ();
			}
			return instance;
		}
	}

	public virtual void Dispose(){

	}
}