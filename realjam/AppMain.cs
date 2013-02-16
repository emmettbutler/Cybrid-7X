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

      var scene = new Scene();
      scene.Camera.SetViewFromViewport();

      var collider = new Collider();

      var player = new Player(new Vector2(40,10));
      scene.AddChild(player.sprite);
      collider.add(player);

      SpawnManager spawnmngr = new SpawnManager(scene,collider);

      var goal = new WinSection(new Vector2(scene.Camera.CalcBounds().Max.X-50,scene.Camera.CalcBounds().Max.Y-50),
                                spawnmngr,scene);
      scene.AddChild(goal.sprite);
      //scene.AddChild(goal.spriteoverlay);
      collider.add(goal);
      goal.startNewGoal();

      var trash = new TrashCan(new Vector2(scene.Camera.CalcBounds().Min.X+60,scene.Camera.CalcBounds().Max.Y-50),spawnmngr);
      scene.AddChild(trash.sprite);
      collider.add(trash);

      Director.Instance.RunWithScene(scene, true);

      Scheduler.Instance.Schedule(scene,spawnmngr.FrameUpdate,0.0f,false);
      Scheduler.Instance.Schedule(scene,player.Tick,0.0f,false);

			while (!Input2.GamePad0.Cross.Press) {
        SystemEvents.CheckEvents();
        Director.Instance.Update();
        Director.Instance.Render();

        collider.CollideWalls(scene);
        collider.Collide();

        if(spawnmngr.cellsOverLimit()){
          GameReset();
        }

        Director.Instance.GL.Context.SwapBuffers();
        Director.Instance.PostSwap();
			}
      Director.Terminate();
		}
    public static void GameReset(){
        Director.Instance.ReplaceScene(new GameOverScene());
    }
	}
}
