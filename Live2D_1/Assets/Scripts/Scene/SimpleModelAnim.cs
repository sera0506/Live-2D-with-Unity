using UnityEngine;
using System;
using System.Collections;
using live2d;
using live2d.framework;

public class SimpleModelAnim : MonoBehaviour {

	private Live2DModelUnity live2DModel;
	private Live2DMotion motion;
	private MotionQueueManager motionMgr;
	
	private Matrix4x4 live2DCanvasPos;
	
	public TextAsset mocFile ;
	public Texture2D[] textureFiles ;
	public TextAsset motionFile;
	
	private Animator anim;    // Animator
	private MotionBehaviour mtnBehaviour;    // MotionBehaviour
	
	void Start () 
	{
		Live2D.init();
		anim = GetComponent<Animator>();
		mtnBehaviour = anim.GetBehaviour<MotionBehaviour>();
		
		live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);
		
		for (int i = 0; i < textureFiles.Length; i++)
		{
			live2DModel.setTexture(i, textureFiles[i]);
		}
		
		float modelWidth = live2DModel.getCanvasWidth();
		live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);
		
		motionMgr = new MotionQueueManager();
		motion = Live2DMotion.loadMotion(motionFile.bytes);
	}

	void load()
	{
		//讀取model資料
		live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);
		
		for (int i = 0; i < textureFiles.Length; i++)
		{
			//將texture與model關聯起來
			live2DModel.setTexture(i, textureFiles[i]);
		}
		
		//指定live2dmodel的描繪位置
		float modelWidth = live2DModel.getCanvasWidth();
		live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);

	}

	void Update()
	{
		
		if (live2DModel == null) load();
		live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);
		if (!Application.isPlaying)
		{
			live2DModel.update();
			return;
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow))
			this.gameObject.GetComponent<Animator> ().SetInteger ("Motion", 1);
		else if(Input.GetKeyDown (KeyCode.RightArrow))
		{
			this.gameObject.GetComponent<Animator> ().SetInteger ("Motion", 0);
		}

		live2DModel.update();
	}

	void OnRenderObject()
	{
		if (live2DModel == null) return;
		live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);
		
		if ( ! Application.isPlaying)
		{
			live2DModel.update();
			live2DModel.draw();
			return;
		}
		
		// motion播完或是flag改變
		if (motionMgr.isFinished() || mtnBehaviour.changeflg == true)
		{
			// load motion
			motion = Live2DMotion.loadMotion(motionFile.bytes);
			// 播放motion
			motionMgr.startMotion(motion);
			// 設置flag為off
			mtnBehaviour.changeflg = false;
		}
		
		motionMgr.updateParam(live2DModel);
		
		live2DModel.update();
		live2DModel.draw();
	}
}
