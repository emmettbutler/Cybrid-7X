using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using System.Collections.Generic;

namespace realjam {
  public class GameScene:Scene {

    public Player player {get; set;}
    public SpawnManager spawnmngr {get; set;}
    public Collider collider {get; set;}
    public Boolean isRaining {get; set;}
    public List<SpriteTile> rain {get; set;}

    public GameScene() {
      Camera.SetViewFromViewport();

      isRaining = false;

      var bg = Support.TiledSpriteFromFile("/Application/assets/Background_Object.png", 1, 1);
      bg.Position = new Vector2((Camera.CalcBounds().Max.X)/2,(Camera.CalcBounds().Max.Y)/2);
      bg.CenterSprite();
      bg.VertexZ = 0;
      this.AddChild(bg,0);

      Random rng = new Random();

      collider = new Collider();

      SpawnManager.Instance = new SpawnManager(this,collider);

      player = new Player(new Vector2(40,10));
      AddChild(player.sprite);
      collider.add(player);

      var fencefront = Support.TiledSpriteFromFile("/Application/assets/Fence_Front.png", 1, 1);
      fencefront.Position = new Vector2((Camera.CalcBounds().Max.X)/2,(Camera.CalcBounds().Min.Y+50)/2);
      fencefront.CenterSprite();
      fencefront.VertexZ = 1;
      this.AddChild(fencefront,0);
      Console.WriteLine(fencefront.Position);

      if(isRaining){
        rain = new List<SpriteTile>();
        var rainsprite = Support.TiledSpriteFromFile("/Application/assets/rain_Sheet.png", 9, 1);
        var raincountX = Camera.CalcBounds().Max.X/rainsprite.Quad.X.X;
        var raincountY = Camera.CalcBounds().Max.Y/rainsprite.Quad.Y.Y;
        var currentPos = new Vector2(0, 20);
        for(var i = 0; i < raincountY; i++){
          for(int j = 0; j < raincountX; j++){
            rainsprite = Support.TiledSpriteFromFile("/Application/assets/rain_Sheet.png", 9, 1);
            rainsprite.CenterSprite();
            rainsprite.Position = currentPos;
            rainsprite.VertexZ = 1;
            this.AddChild(rainsprite,1);

            rain.Add(rainsprite);
            var RainAnimation = new Support.AnimationAction(rainsprite, 9, 1, 1.0f, looping: true);
            var Wait = new DelayTime((float)rng.NextDouble() * 2);
            var seq = new Sequence();
            seq.Add(Wait);
            seq.Add(RainAnimation);
            rainsprite.RunAction(seq);

            currentPos += new Vector2(rainsprite.Quad.X.X, 0);
          }
          currentPos = new Vector2(0, currentPos.Y + rainsprite.Quad.Y.Y);
        }
      }

      var goal = new WinSection(new Vector2(Camera.CalcBounds().Max.X-100,Camera.CalcBounds().Max.Y-100),
                                spawnmngr,this);
      AddChild(goal.sprite);
      collider.add(goal);
      goal.startNewGoal();

      var goaloverlay = Support.TiledSpriteFromFile("/Application/assets/Screen_Object.png", 10, 4);
      goaloverlay.CenterSprite();
      goaloverlay.Position = new Vector2(goal.sprite.Position.X, goal.sprite.Position.Y+35);
      goaloverlay.VertexZ = 1;
      this.AddChild(goaloverlay);
      var ScreenAnimation = new Support.AnimationAction(goaloverlay, 1, 40, 1.0f, looping: true);
      goaloverlay.RunAction(ScreenAnimation);
    }
  }
}

