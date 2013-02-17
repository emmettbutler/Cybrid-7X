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
    public SpriteTile spriteoverlay, goalSprite;
    public int goalMutation {get; set;}

    public WinSection(Vector2 pos) : base(pos) {
      sprite = Support.TiledSpriteFromFile("/Application/assets/Goal_Object.png", 1, 1);
      spriteoverlay = Support.TiledSpriteFromFile("/Application/assets/Goal_Object_Overlay_1.png", 1, 1);
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
          if(this.checkCellWin(collisionCells[i])){
            Console.WriteLine("WINNER");
          } else {
            Console.WriteLine("WRONG ONE");
          }
        }
      }
    }

    public override void CollideFrom(GameEntity instance){

    }

    public void startNewGoal(){
      int[] goalIndices = new int[10];
      Random rng = new Random();
      goalMutation = 0;

      for(int i = 0; i < 10; i++){
        if(i < 2){
          goalIndices[i] = rng.Next(1,7);
          goalMutation += (int)goalIndices[i]*((int)System.Math.Pow(10, i));
        }
      }

      String spritePath = "/Application/assets/cell_";
      spritePath += goalMutation.ToString("D10") + ".png";

      goalSprite = Support.TiledSpriteFromFile(spritePath, 1, 1);

      goalSprite.CenterSprite();
      goalSprite.Position = sprite.Position;
      goalSprite.Position = new Vector2(sprite.Position.X + 10, sprite.Position.Y + 45);
      goalSprite.VertexZ = 1;
      goalSprite.Quad.S = (goalSprite.TextureInfo.TextureSizef)/2;
      GameScene.Instance.AddChild(goalSprite);
    }

    public Boolean checkCellWin(Cell c){
      Sequence seq;
      if (c.type == goalMutation){
        goalSprite.Visible = false;
        var right = Support.TiledSpriteFromFile("/Application/assets/display_BAD.png", 1, 1);
        GameScene.Instance.AddChild(right);
        right.CenterSprite();
        right.Quad.S = right.TextureInfo.TextureSizef/2;
        right.Position = goalSprite.Position;
        right.VertexZ = 1;
        seq = new Sequence();
        seq.Add(new DelayTime(2));
        seq.Add(new CallFunc(() => { GameScene.Instance.RemoveChild(right, true); goalSprite.Visible = true; }));
        GameScene.Instance.RunAction(seq);
        return true;
      }
      goalSprite.Visible = false;
      var wrong = Support.TiledSpriteFromFile("/Application/assets/display_BAD.png", 1, 1);
      wrong.CenterSprite();
      wrong.Quad.S = wrong.TextureInfo.TextureSizef/2;
      wrong.Position = goalSprite.Position;
      wrong.VertexZ = 1;
      GameScene.Instance.AddChild(wrong);
      seq = new Sequence();
      seq.Add(new DelayTime(2));
      seq.Add(new CallFunc(() => { GameScene.Instance.RemoveChild(wrong, true); goalSprite.Visible = true; }));
      GameScene.Instance.RunAction(seq);
      return false;
    }
  }
}

