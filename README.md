# SimpleRunGame
このゲームはより高いスコア目指してプレイヤーを移動、ジャンプさせるシンプルなランゲームです。
また、このリポジトリにはAdobe社が提供するMixamoの3Dモデルおよびアニメーションを使用しています。
Mixamoアセットはライセンス上、再配布が認められていないため、本プロジェクトには含まれていません。
ご利用にあたってはお手数をおかけいたしますが、Mixamo([https://www.mixamo.com/#/](https://www.mixamo.com/#/))より以下のアセットを取得し所定の操作が必要になります。

# 操作方法
| Key |  Action  |
| ---- | ---- |
|  A  |  右へプレイヤーが移動 |
|  D  |  右へプレイヤーが移動 |
|  Space  |  ジャンプ |

# 開発期間

2025/04/26 ~ 2025/05/10  

# 開発環境
|  Tools  |  Version  |
| ---- | ---- |
|  Unity  |  2022.3.56f1 |
|  Visual Studio |  2022  |

|  PakageName  |  Version  |
| ---- | ---- |
|  Universal RP  |  14.0.11   |
|  UniRx  |  7.1.0  |
|  UniTask  |  2.5.10  |

# 使用アセット
|  AssetName  |  Version  |
| ---- | ---- |
|  [DOTween (HOTween v2)](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)  |  1.2.765  |

### MixsamoAssets
| Model | Animation |
| ---- | ---- |
| Y Bot |Jump(RootT.yの0:00以外のキーを削除)|
| Y Bot |Running|
| Y Bot |Stumble Backwards(RootT.zのキーを削除)|

# 命名規則
|public|private|local|function|
|:--:|:--:|:--:|:--:|
|Upper|_+lower|lower|Upper|

