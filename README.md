# SCPSwap ![Latest release](https://img.shields.io/github/v/release/EscapeTheFacility/SCPSwap) ![Downloads](https://img.shields.io/github/downloads/EscapeTheFacility/SCPSwap/total) 
This is a port of the SCPSwap plugin by DentyTxR (and BuildBoy12) to NWAPI. Most of the code has been taken from the original repo.
Currently the CustomSwaps API has been removed due to issues while loading, it will be reinplemented in the future.

DentyTxR's version of the plugin (for EXILED) can be found [here](https://github.com/DentyTxR/ScpSwap).

### Features
- Allows players to swap between SCP roles with commands
- Configuration on allowed swap roles, timings and messages
- <s>API to allow other plugins to integrate into the swap feature</s>

## Permissions
This plugin requires NWPS permissions to function, it is shipped as a dependency.
- `scpswap.swap` to allow swapping.
- `scpswap.swap` AND `scpswap.any` to allow swapping to new SCPs.

## Default configuration
```yaml
is_enabled: true
debug: false
# The duration, in seconds, before a swap request gets automatically deleted.
request_timeout: 20
# The duration, in seconds, after the round starts that swap requests can be sent.
swap_timeout: 60
# A collection of roles blacklisted from being swapped to.
blacklisted_scps:
- Scp0492
```

## Default messages
```yaml
# A collection of custom names with their correlating RoleType.
translatable_swaps:
  173: Scp173
  peanut: Scp173
  939: Scp939
  079: Scp079
  79: Scp079
  computer: Scp079
  pc: Scp079
  106: Scp106
  larry: Scp106
  096: Scp096
  96: Scp096
  shyguy: Scp096
  049: Scp049
  49: Scp049
  doctor: Scp049
  0492: Scp0492
  492: Scp0492
  zombie: Scp0492
# The message to be displayed to all Scp subjects at the start of the round.
start_message:
  message: <color=yellow><b>Did you know you can swap classes with other SCP's?</b></color> Simply type <color=orange>.scpswap (role number)</color> in your in-game console (not RA) to swap!
  duration: 15
# The broadcast to display to the receiver of a swap request.
request_broadcast:
  message: >-
    <i>You have an SCP Swap request!

    Check your console by pressing [`] or [~]</i>
  duration: 5
# The console message to send to the receiver of a swap request.
request_console_message:
  message: You have received a swap request from $SenderName who is $RoleName. Would you like to swap with them? Type ".scpswap accept" to accept or ".scpswap decline" to decline.
  color: yellow
# The console message to send to players when the swap succeeds.
swap_successful:
  message: Swap successful!
  color: green
# The console message to send to the receiver of a swap request that has timed out.
timeout_receiver:
  message: Your swap request has timed out.
  color: red
# The console message to send to the sender of a swap request that has timed out.
timeout_sender:
  message: The player did not respond to your request.
  color: red

```
