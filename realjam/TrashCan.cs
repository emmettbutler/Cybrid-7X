using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;
using System.Collections.Generic;

namespace realjam {
  public class TrashCan:GameEntity {

    private float radius;
    public List<Cell> collisionCells;

    public TrashCan(Vector2 pos) : base(pos) {
      sprite = Support.TiledSpriteFromFile("/Application/assets/Trash_Object.png", 1, 1);
      sprite.Position = pos;
      sprite.CenterSprite();
    }

    public override float GetRadius (){
      radius = this.sprite.Quad.X.X;
      return radius/2;
    }

    public override void CollideTo(GameEntity instance){
      collisionCells = new List<Cell>();
      if(instance is Cell){
        Cell c = (Cell)instance;
        if(c.grabbed){
          collisionCells.Add(c);
          Vector2 cellCenter = c.GetCenter();
  
          for(var i = 0; i < collisionCells.Count; i++){
            //s.DestroyCell(collisionCells[i]);
            Support.SoundSystem.Instance.Play("trash.wav");
            collisionCells[i].Visible = false;
            collisionCells[i].destroyed = true;
            Console.WriteLine("TRASH!!!");
          }
        }
      }
    }

    public override void CollideFrom(GameEntity instance){

    }
  }
}

