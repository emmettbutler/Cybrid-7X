using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace realjam
{
	public class AppMain
	{
		
		public static void Main (string[] args)
		{
      Director.Initialize();
      Director.Instance.GL.Context.SetClearColor(new Vector4(36.0f/255.0f, 234.0f/255.0f, 143.0f/255.0f, 1.0f));

      Director.Instance.GL.Context.Enable(EnableMode.DepthTest);
      Director.Instance.GL.Context.Enable(EnableMode.Blend);
      Director.Instance.GL.Context.SetBlendFuncAlpha(BlendFuncMode.ReverseSubtract, BlendFuncFactor.OneMinusSrcAlpha, BlendFuncFactor.OneMinusSrcAlpha);

      Collider.Instance = new Collider();
      SpawnManager.Instance = new SpawnManager();
      GameScene.Instance = new GameScene();
      GameScene.Instance.setup();
      SpawnManager.Instance.setup();

      HelloScene introScene = new HelloScene();

      Director.Instance.RunWithScene(introScene, true);

      Scheduler.Instance.Schedule(introScene,introScene.Tick,0.0f,false);

			while (!Input2.GamePad0.Cross.Press) {
        SystemEvents.CheckEvents();
        Director.Instance.Update();
        Director.Instance.Render();

        Collider.Instance.CollideWalls();
        Collider.Instance.Collide();

        Director.Instance.GL.Context.SwapBuffers();
        Director.Instance.PostSwap();
			}
      Director.Terminate();
		}
	}
}
