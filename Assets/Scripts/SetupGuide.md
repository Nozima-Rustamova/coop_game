# Multiplayer Food Collection Game - Setup Guide

## New Features Added

### 1. Cart Consumption Animation ✅
- Items now shrink and move towards the cart when collected
- Configurable consumption time and animation curve
- Items are destroyed after being fully consumed

### 2. Player Interaction System ✅
- **Slapping**: Press SPACE to slap nearby players (2 unit range)
- **Knockback**: Players get knocked back when slapped or hit by food
- **Cooldown**: 1-second cooldown between slaps
- **Audio**: Support for slap and hit sounds

### 3. Enhanced Food Physics ✅
- Food can hit and knock back other players
- Food has lifetime (10 seconds) before disappearing
- Better collision detection for player hits
- Food is destroyed after hitting a player

### 4. Cart Visual Feedback ✅
- Particle effects when collecting items
- Audio feedback for collections
- Display of collected items in cart (up to 5 items)
- Floating animation for displayed items

## Setup Instructions

### For Each Player GameObject:

1. **Add Required Components:**
   - `PlayerMovement` (already exists)
   - `PlayerItemHandler` (already exists)
   - `PlayerInteraction` (NEW - add this script)
   - `Rigidbody` (for physics)
   - `Collider` (for collision detection)
   - `AudioSource` (for sound effects)

2. **Configure PlayerInteraction:**
   - Set `slapRange` to 2.0
   - Set `slapForce` to 10.0
   - Set `slapCooldown` to 1.0
   - Set `knockbackForce` to 8.0
   - Set `knockbackDuration` to 0.5
   - Assign audio clips for `slapSound` and `hitSound`

3. **Player Tags:**
   - Player 1: Tag = "Player_1"
   - Player 2: Tag = "Player_2"

### For Cart Zones:

1. **Add Required Components:**
   - `CartZone` (already exists)
   - `Collider` (set as Trigger)
   - `AudioSource` (for collection sounds)
   - `ParticleSystem` (for collection effects)

2. **Configure CartZone:**
   - Set `consumptionTime` to 1.0
   - Set `consumptionPoint` to desired consumption location
   - Assign `collectEffect` (ParticleSystem)
   - Assign `collectSound` (AudioClip)
   - Create empty GameObject as `itemDisplayParent` for showing collected items
   - Set `maxDisplayItems` to 5
   - Set `itemSpacing` to 0.5

### For Food Items:

1. **Add Required Components:**
   - `FoodItem` (already exists)
   - `Rigidbody` (for physics)
   - `Collider` (for collision detection)

2. **Configure FoodItem:**
   - Set `hitForce` to 5.0
   - Set `lifetime` to 10.0
   - Tag = "Food"

### Input Configuration:

- **Player 1 Movement:** WASD or Arrow Keys
- **Player 2 Movement:** Configure in Input Manager
- **Pickup:** E key
- **Throw:** Q key
- **Slap:** SPACE key

### Physics Settings:

- Ensure players have Rigidbodies with appropriate mass
- Set up proper collision layers
- Configure physics materials for realistic bouncing

## Gameplay Features:

1. **Collection System:**
   - Players pick up food with E key
   - Throw food with Q key
   - Food must be thrown into own cart to score
   - Cart shows visual feedback and collected items

2. **Combat System:**
   - Players can slap each other when close (SPACE key)
   - Food can hit and knock back other players
   - Knockback disables movement temporarily

3. **Visual Feedback:**
   - Items shrink and disappear when collected
   - Particle effects on collection
   - Audio feedback for all actions
   - Cart displays collected items with floating animation

## Tips for Testing:

1. Create two player prefabs with all required components
2. Set up two cart zones with proper triggers
3. Spawn food items using the SpawnManager
4. Test pickup, throwing, slapping, and collection mechanics
5. Adjust force values and timing for desired feel

## Next Steps (Optional Enhancements):

- Add power-ups or special items
- Implement different food types with different effects
- Add screen shake effects
- Create better visual effects and animations
- Add multiplayer networking support
- Implement different game modes
