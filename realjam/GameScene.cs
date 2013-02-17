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

    public static GameScene Instance;

    public Player player {get; set;}
    public Collider collider {get; set;}
    public Boolean isRaining {get; set;}
    public List<SpriteTile> rain {get; set;}
    public DateTime borntime;
    public float ttime;
    public int itime;
    private Random rng;

    public GameScene() {
      borntime = DateTime.UtcNow;
    }

    public void setup(){
      Camera.SetViewFromViewport();

      isRaining = false;
      itime = 0;
      rain = new List<SpriteTile>();

      var bg = Support.TiledSpriteFromFile("/Application/assets/Background_Object.png", 1, 1);
      bg.Position = new Vector2((Camera.CalcBounds().Max.X)/2,(Camera.CalcBounds().Max.Y)/2);
      bg.CenterSprite();
      bg.VertexZ = 0;
      this.AddChild(bg,0);

      rng = new Random();

      player = new Player(new Vector2(40,10));
      AddChild(player.sprite);
      Collider.Instance.add(player);

      var fencefront = Support.TiledSpriteFromFile("/Application/assets/Fence_Front.png", 1, 1);
      fencefront.Position = new Vector2((Camera.CalcBounds().Max.X)/2,(Camera.CalcBounds().Min.Y+50)/2);
      fencefront.CenterSprite();
      fencefront.VertexZ = 1;
      this.AddChild(fencefront,0);
      Console.WriteLine(fencefront.Position);

      var goal = new WinSection(new Vector2(Camera.CalcBounds().Max.X-100,Camera.CalcBounds().Max.Y-100));

      Collider.Instance.add(goal);
      goal.startNewGoal();

      var trash = new TrashCan(new Vector2(GameScene.Instance.Camera.CalcBounds().Min.X+90,GameScene.Instance.Camera.CalcBounds().Max.Y-100));
      GameScene.Instance.AddChild(trash.sprite);
      Collider.Instance.add(trash);
    }

    public void Tick(float dt){
      TimeSpan t = DateTime.UtcNow - borntime;
      ttime = (float) t.TotalMilliseconds/1000.0f;

      itime += 1;

      Boolean timerInterval = itime >= 1300 && (itime % 1300 == 0);
      if(!isRaining){
        if(timerInterval){
          Console.WriteLine("switch on");
          isRaining = true;
          flipRaining();
        }
      } else {
        if(timerInterval){
          Console.WriteLine("switch off");
          isRaining = false;
          flipRaining();
        }
      }
    }

    public void flipRaining(){
      if(isRaining){
        Support.SoundSystem.Instance.Play("rain.wav");
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
      } else {
        for(int i = 0; i < rain.Count; i++){
          GameScene.Instance.RemoveChild(rain[i], true);
        }
        rain.Clear();
      }
    }
  }
}

