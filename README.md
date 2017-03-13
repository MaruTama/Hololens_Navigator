# HoloLensを用いた進路の3D表示
![route_display_1](https://cloud.githubusercontent.com/assets/13286706/23851614/72d7816c-0827-11e7-916a-b9e1b37d9acb.gif)

## 目的
　使用者が現在の進み具合やどのような経路を進むのかを視覚的に理解できる。また、予想到着時間等の情報も表示し使用者が情報へのアクセスが容易にできるようにする。

## 目標
- 校内の廊下でHoloLens上に経路情報から3Dの経路線を表示する。
- 壁に接近したらアラートを出す。

## 仕様
　Unityを用いてHolographic(空間上に3Dオブジェクトを配置する)アプリ開発を行う。

## HoloLensで取得できる情報
### unityから取得できる情報
- 自位置と壁の距離
- 自位置を含む任意の点のワールド座標の取得
- Rayを飛ばし衝突した際の面の角度
- 空間マッピング
- 取得済み3Dモデルとの比較から目的地の座標を取得する <- 現在のマッピングデータとモデルとの較正が済んでいるとき

### 搭載するハードウェアと取得できる情報
- 慣性計測ユニット(IMU)：加速度、ジャイロ、方位測定
- 環境認識カメラ：(おそらくマッピング用)
- depthセンサー：ジェスチャー認識用
- 複合現実感キャプチャ：？
- マイク：音声認識
- スピーカー：音声出力、三次元音響
- Gaze：ヘッドトラッキング
- Air Tap：セレクト、タップ
- Bluetooth：Bluetooth 4.0 LE
- 無線LAN：Wi-Fi 802.11ac



参考文献 1：[MRとは？ HoloLensのハードウェア／機能／アプリ動作／ユーザー操作](http://www.buildinsider.net/small/hololens/001   "MRとは？ HoloLensのハードウェア／機能／アプリ動作／ユーザー操作")  
参考文献 2：[HOLOLENSとは、実機説明](https://azure-recipe.kc-cloud.jp/2016/12/about-hololens/   "HOLOLENSとは、実機説明")

## 実装したこと
- Unityを使用してHoloLens上のアプリを作成する。
- HoloLensを用いて校内のモデル化(マッピング)。 -> 図1
- unity上でのモデルの読み込み
- Raycastを用いた物体との距離測定。-> 図2
- 視野へのコンポーネントの追従(情報表示用)。 -> 図3
- 回帰直線の関数の作成(壁の直線化に用いる) -> 図4
- 現在の壁にモデルの壁を合わせる(LookRotationにて軸を揃えた) -> 手順1
- モデル上に経路を表示する -> 図5
- 移動時に(3Dモデルの)壁に接近した際に、接近している方向から音を鳴らすようにした。近いほど音の周期が早くなる。
- World Anchorというオブジェクトの座標を固定する機能を使用し、一度壁に合わせたら二度目以降のアプリ起動時には壁の調整が不要にした。
- World Anchorを使用した事により、座標がしっかり固定され、壁に接近しているという判定が正確になった。

![3d_model](https://cloud.githubusercontent.com/assets/13286706/23851664/acd68b4c-0827-11e7-9d67-793f75920db8.gif)  
図1 校内のモデル化

<blockquote class="twitter-video" data-lang="ja"><p lang="und" dir="ltr"><a href="https://t.co/MkcysKBRAB">pic.twitter.com/MkcysKBRAB</a></p>&mdash; たま (@mukimuki_tamago) <a href="https://twitter.com/mukimuki_tamago/status/841206373664735232">2017年3月13日</a></blockquote>
<script async src="//platform.twitter.com/widgets.js" charset="utf-8"></script>  
図2 距離取得

<blockquote class="twitter-video" data-lang="ja"><p lang="und" dir="ltr"><a href="https://t.co/WdiTXPqz78">pic.twitter.com/WdiTXPqz78</a></p>&mdash; たま (@mukimuki_tamago) <a href="https://twitter.com/mukimuki_tamago/status/841206669279285248">2017年3月13日</a></blockquote>
<script async src="//platform.twitter.com/widgets.js" charset="utf-8"></script>  
図3 コンポーネントの追従

<img width="1440" alt="ex_regression_line" src="https://cloud.githubusercontent.com/assets/13286706/23851699/c2eedb46-0827-11e7-9f36-dd39ec2bc60e.png">
図4 回帰直線の作成  
参考文献 3：[回帰直線](http://physmath.main.jp/src/st-regression-line.html " 回帰直線")  

![route_display_0](https://cloud.githubusercontent.com/assets/13286706/23851756/f3a9197c-0827-11e7-8edd-53e94c8fd586.gif)
<- 現在の壁の回帰直線を平面に揃え、現在の壁と3Dモデルとの較正を行う。  
![route_display_1](https://cloud.githubusercontent.com/assets/13286706/23851614/72d7816c-0827-11e7-916a-b9e1b37d9acb.gif)
<- 目的地へと経路表示が行われている。   
図5 経路の表示


### 手順1
現在の壁の回帰直線の表示を平面上で揃え、AirTapするとキャリアブレーションを行い、現在の空間に対してモデルの位置を合わせる。以下に動作風景を示す。

アプリを起動し現在の壁の回帰直線を表示する。  
![start](https://cloud.githubusercontent.com/assets/13286706/23851797/1efb6efe-0828-11e7-804a-49870ca3e6e0.gif)  
視点を変えて、壁の回帰直線が変わる様子。  
![calibration](https://cloud.githubusercontent.com/assets/13286706/23851823/380a7606-0828-11e7-81df-defd3c052471.gif)  
現在空間(奥行きは真ん中)と3Dモデルの空間(奥行き左)との差の画像.  
<img width="1440" alt="difference" src="https://cloud.githubusercontent.com/assets/13286706/23851836/44bf94bc-0828-11e7-9633-6ff2716cf8a5.png">
壁の平面でAirTapをすると、現在の壁とモデルの壁が揃う。  
![seted](https://cloud.githubusercontent.com/assets/13286706/23851847/53bc4488-0828-11e7-82aa-50130a3c0375.gif)
