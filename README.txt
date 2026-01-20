Чому я робив таку архітектуру?

------------
інпут система: чіткий поділ intent / logic / state
InputSystem
   ↓
PlayerWeaponInput (IWeaponInputSource)
   ↓
PlayerWeaponController (orchestrator)
   ↓
WeaponInventory (state + lifecycle)
   ↓
Gun / RuntimeData

------------
PlayerWeaponInput
input = intent
жодних слотів / Gun / логіки
event-based, без Update polling
Swap - окремий intent

PlayerWeaponController
Controller не знає як працює inventory
Controller не знає звідки input
AimSystem підключений через евент
не є год обьектом

WeaponInventory
тільки тут зберігається реф на ган
RuntimeData зберігається між drop / pickup
Reload відміняється on drop через токен(reloadVersion)
далі: IWeaponInventory
Gun CurrentGun
bool CanSwap
void Swap()

Gun + WeaponRuntimeData
Gun = поведінка
GunConfig = SO data
WeaponRuntimeData = state
Events для UI

------------
Damage system
IDamageable інтерфейс для любого обьекту (бочки, мішені, люди)
IDamageInstigator не впевнений чи треба але що б не робити зайві лінки
DamageInfo структура з всіма данними (як в кс кілфід хто-чим-кого-як)

------------
UIContext = singleton ультра хз мені не подобається але краще не вигадав підходу

------------
LevelManager
FindAnyObjectByType<InputSystem.InputsManager> тоже як будто піво


TODO-----TODO-----TODO-----TODO
WeaponInventory 2 slots + swap
CanShoot / CanSwap як state

Player Input
↓
WeaponController
↓
WeaponStateMachine NEW але чи треба?
↓
WeaponInventory (2 slots)
↓
Gun (exec only)

WeaponInventory
 - зберігати 2 слоти
 - знати активний слот
 - НЕ знати про input - вже
 - НЕ знати про cooldown / reload?? бо буде стейт машина

WeaponStateMachine (DECISION LAYER)

🎯 Відповідальність
вирішує чи можна
-Shoot
-Reload
-Swap
-Drop
-централізує state
Idle
Shooting
Reloading
Swapping
Disabled (LevelComplete / Death)

у стейтмашину підуть всі перевірки з гану, тому він буде просто
Shoot()
StartReload()
CancelReload()
