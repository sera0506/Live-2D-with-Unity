using UnityEngine;
using System;
using System.Collections;
using live2d;
using live2d.framework;

public class SimpleModel : MonoBehaviour {

	public TextAsset mocFile;
	public TextAsset physicsFile;
	public Texture2D[] textureFiles;
	
	private Live2DModelUnity live2DModel;
	private EyeBlinkMotion eyeBlink = new EyeBlinkMotion();
	private L2DTargetPoint dragMgr = new L2DTargetPoint();
	private L2DPhysics physics;
	private Matrix4x4 live2DCanvasPos;

	void Start()
	{
		Live2D.init();
		
		load();
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

		//載入物理運算檔
		if (physicsFile != null) physics = L2DPhysics.load(physicsFile.bytes);
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
		
		var pos = Input.mousePosition;
		//滑鼠左鍵壓下事件
		if (Input.GetMouseButtonDown(0))
		{
			//
		}
		else if (Input.GetMouseButton(0))
		{
			dragMgr.Set(pos.x / Screen.width * 2 - 1, pos.y / Screen.height * 2 - 1);
		}
		else if (Input.GetMouseButtonUp(0))
		{
			dragMgr.Set(0, 0);
		}
		
		
		dragMgr.update();
		//臉部方向的調整
		live2DModel.setParamFloat("PARAM_ANGLE_X", dragMgr.getX() * 30);
		live2DModel.setParamFloat("PARAM_ANGLE_Y", dragMgr.getY() * 30);

		//身體方向調整
		live2DModel.setParamFloat("PARAM_BODY_ANGLE_X", dragMgr.getX() * 10);

		//眼睛看的方向調整
		live2DModel.setParamFloat("PARAM_EYE_BALL_X", -dragMgr.getX());
		live2DModel.setParamFloat("PARAM_EYE_BALL_Y", -dragMgr.getY());
		
		double timeSec = UtSystem.getUserTimeMSec() / 1000.0;
		double t = timeSec * 2 * Math.PI;
		live2DModel.setParamFloat("PARAM_BREATH", (float)(0.5f + 0.5f * Math.Sin(t / 3.0)));
		
		eyeBlink.setParam(live2DModel);
		
		if (physics != null) physics.updateParam(live2DModel);
		
		live2DModel.update();
	}
	
	
	void OnRenderObject()
	{
		if (live2DModel == null) load();
		if (live2DModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW) live2DModel.draw();
	}
}
