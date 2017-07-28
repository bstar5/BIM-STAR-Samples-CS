import clr
from Umo3D import *
from SharpDX import *
from DemoApp import *

thumbtackNode = None
enableThumbtack = True

def GetThumbtackNode(thumbtackKey) :
    if(thumbtackNode == None) :
        return None
    region = thumbtackNode.Content
    for node in region : 
        if(node.Name == thumbtackKey) :
            return node
    return None

#显示图钉
def ShowThumbtack(thumbtackInfo) : 
    global thumbtackNode
    if(thumbtackNode == None) :
        thumbtackNode = SceneNode()
        thumbtackNode.Content = SpriteRoot("Thumbtack")
        if(enableThumbtack) :
            sceneRoot.Add(thumbtackNode)

    node = GetThumbtackNode(thumbtackInfo.Key)
    if(node != None) :
        return
    region = thumbtackNode.Content
    rect3D = Rect3D(Vector3(0,-30,0),60,60)
    model3D = FloaterModel3D(rect3D,Color.Transparent)
    model3D.FaceToCamera = True
    model3D.ImageSource = engine.TextureManager.LoadFromFile(thumbtackInfo.ImagePath)

    node = SpriteNode(model3D, str(thumbtackInfo.Key))
    node.Transform.SetMatrix(Matrix.Translation(thumbtackInfo.X,thumbtackInfo.Y,thumbtackInfo.Z))
    region.Add(node)

#移除图钉
def RemoveThumbtack(thumbtackKey) : 
    node = GetThumbtackNode(thumbtackKey)
    if(node != None) : 
        node.RemoveSelf()