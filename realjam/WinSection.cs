using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;
using System.Collections.Generic;

namespace realjam {
  public class WinSection:GameEntity {

    private float radius;
    private Vector2 displacement;
    public List<Cell> collisionCells;
    public SpawnManager s;
    public SpriteTile spriteoverlay;
    public Scene scene;
    public SpriteTile goalSprite
    public int goalMutation {get; set;}

    public WinSection(Vector2 pos, SpawnManager s, Scene scene) : base(pos) {
      this.s = s;
      sprite = Support.TiledSpriteFromFile("/Application/assets/Goal_Object.png", 1, 1);
      spriteoverlay = Support.TiledSpriteFromFile("/Application/assets/Goal_Object_Overlay_1.png", 1, 1);
      this.scene = scene;
      sprite.Position = pos;
      spriteoverlay.Position = pos;
      sprite.CenterSprite();
      spriteoverlay.CenterSprite();
    }

    public override float GetRadius (){
      radius = this.sprite.Quad.X.X;
      return radius/2;
    }

    public override void CollideTo(GameEntity instance){
      collisionCells = new List<Cell>();
      if(instance is Cell){
        Cell c = (Cell)instance;
        collisionCells.Add(c);
        Vector2 cellCenter = c.GetCenter();
        Vector2 trashCenter = this.GetCenter();
        displacement = trashCenter - cellCenter;
  
        for(var i = 0; i < collisionCells.Count; i++){
          Console.WriteLine("WINNING???");
        }
      }
    }

    public override void CollideFrom(GameEntity instance){

    }

    public void startNewGoal(){
      goalMutation = 1;
      goalSprite =
    }
  }
}

