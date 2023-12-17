(deftemplate game (multislot name))
(deftemplate choosenGame (multislot name))
(deftemplate resultMessage 
	(multislot message)
	(multislot answer)
	(slot rule-id))

(deftemplate platform (slot name))
(deftemplate single_or_multiplayer (slot type))
(deftemplate paid_or_free (slot type))
(deftemplate genre (slot type))

(deffacts games
	(game (name Counter Strike 2))
	(game (name Minecraft))
	(game (name Valorant))
	(game (name FIFA 2024))
	(game (name Forza Horizon 5))
	(game (name Titanfall 2))
	(game (name Battlefield 1))
	(game (name Dead Space))
	(game (name Mass Effect 2))
	(game (name God of War))
	(game (name Death Stranding))
	(game (name Skyrim))
	(game (name Fallout 4))
	(game (name Fallout 76))
	(game (name The Last of Us))
	(game (name Payday 2))
	(game (name Half-Life))
	(game (name Portal))
	(game (name Team Fortress))
	(game (name Garry's Mod))
	(game (name Rust))
	(game (name GTA 5))
	(game (name Max Payne 3))
	(game (name Dota 2))
	(game (name Warcraft 3))
	(game (name Little Nightmares))
	(game (name Formula 1 2023))
	(game (name NHL 24))
	(game (name HITMAN 3))
	(game (name The Forest))
	(game (name Teardown))
	(game (name Witcher 3))
	(game (name Bloodborne))
	(game (name Atomic Heart))
	(game (name Cyberpunk 2077))
)

(defrule no-result
	(declare (salience 10))
	=> 
	(assert (resultMessage (rule-id no-result) (message No Match)))
) 

(defrule rule1
	(declare (salience 40))
	(choosenGame (name Counter Strike 2))
=> 
	(assert (platform (name PC)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (paid_or_free (type free)))
	(assert (genre (type shooter)))
	(assert (resultMessage (rule-id rule1) (message Counter Strike 2: PC multiplayer free shooter))) 
	(halt)
)

(defrule rule2 
	(declare (salience 40))
	(platform (name PC))
	(single_or_multiplayer (type multiplayer))
	(paid_or_free (type free))
	(genre (type shooter))
=> 
	(assert (resultMessage (message Counter Strike 2) (rule-id rule2)))
	(halt)
)

(defrule rule3 
	(declare (salience 40))
	(choosenGame (name Minecraft))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type free)))
	(assert (genre (type survival)))
	(assert (genre (type sandbox)))
	(assert (genre (type open-world)))
	(assert (resultMessage (rule-id rule3) (message Minecraft: PC/PS/XBOX single/multiplayer free survival sandbox open-world)))
	(halt)
)

(defrule rule4 
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type multiplayer|single))
	(paid_or_free (type free))
	(genre (type survival))
	(genre (type sandbox))
	(genre (type open-world))
=> 
	(assert (resultMessage (message Minecraft) (rule-id rule4)))
	(halt)
)

(defrule rule5 
	(declare (salience 40))
	(choosenGame (name Valorant))
=> 
	(assert (platform (name PC)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (paid_or_free (type free)))
	(assert (genre (type shooter)))
	(assert (resultMessage (rule-id rule5) (message Valorant: PC multiplayer free shooter)))
	(halt)
)

(defrule rule6 
	(declare (salience 40))
	(platform (name PC))
	(single_or_multiplayer (type multiplayer))
	(paid_or_free (type free))
	(genre (type shooter))
=> 
	(assert (resultMessage (message Valorant) (rule-id rule6)))
	(halt)
)

(defrule rule7 
	(declare (salience 40))
	(choosenGame (name FIFA 2024))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type sport)))
	(assert (resultMessage (rule-id rule7) (message FIFA 2024: PC/PS/XBOX single/multiplayer paid sport)))
	(halt)
)

(defrule rule8 
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type multiplayer|single))
	(paid_or_free (type paid))
	(genre (type sport))
=> 
	(assert (resultMessage (message FIFA 2024) (rule-id rule8)))
	(halt)
)

(defrule rule9
	(declare (salience 40))
	(choosenGame (name Forza Horizon 5))
=> 
	(assert (platform (name PC)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type sport)))
	(assert (genre (type race)))
	(assert (resultMessage (rule-id rule9) (message Forza Horizon 5: PC/XBOX single/multiplayer paid sport race)))
	(halt)
)

(defrule rule10 
	(declare (salience 40))
	(platform (name PC|XBOX))
	(single_or_multiplayer (type multiplayer|single))
	(paid_or_free (type paid))
	(genre (type sport))
	(genre (type race))
=> 
	(assert (resultMessage (message Forza Horizon 5) (rule-id rule10)))
	(halt)
)

(defrule rule11
	(declare (salience 40))
	(choosenGame (name Titanfall 2))
=> 
	(assert (platform (name PC)))
	(assert (platform (name XBOX)))
	(assert (platform (name PS)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type shooter)))
	(assert (genre (type action)))
	(assert (genre (type science-fiction)))
	(assert (resultMessage (rule-id rule11) (message Titanfall 2: PC/PS/XBOX single/multiplayer paid shooter action science-fiction)))
	(halt)
)

(defrule rule12
	(declare (salience 40))
	(platform (name PC|XBOX|PS))
	(single_or_multiplayer (type multiplayer|single))
	(paid_or_free (type paid))
	(genre (type shooter))
	(genre (type action))
	(genre (type science-fiction))
=> 
	(assert (resultMessage (rule-id rule12) (message Titanfall 2)))
	(halt)
)

(defrule rule13
	(declare (salience 40))
	(choosenGame (name Battlefield 1))
=> 
	(assert (platform (name PC)))
	(assert (platform (name XBOX)))
	(assert (platform (name PS)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type shooter)))
	(assert (genre (type action)))
	(assert (resultMessage (rule-id rule13) (message Battlefield 1: PC/PS/XBOX single/multiplayer paid shooter action)))
	(halt)
)

(defrule rule14
	(declare (salience 40))
	(platform (name PC|XBOX|PS))
	(single_or_multiplayer (type multiplayer|single))
	(paid_or_free (type paid))
	(genre (type shooter))
	(genre (type action))
=> 
	(assert (resultMessage (rule-id rule14) (message Battlefield 1)))
	(halt)
)

(defrule rule15
	(declare (salience 40))
	(choosenGame (name Dead Space))
=> 
	(assert (platform (name PC)))
	(assert (platform (name XBOX)))
	(assert (platform (name PS)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type shooter)))
	(assert (genre (type horror)))
	(assert (genre (type action)))
	(assert (genre (type science-fiction)))
	(assert (resultMessage (rule-id rule15) (message Dead Space: PC/PS/XBOX single paid shooter horror action science-fiction)))
	(halt)
)

(defrule rule16
	(declare (salience 40))
	(platform (name PC|XBOX|PS))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type shooter))
	(genre (type horror))
	(genre (type action))
	(genre (type science-fiction))
=> 
	(assert (resultMessage (rule-id rule16) (message Dead Space)))
	(halt)
)

(defrule rule17
	(declare (salience 40))
	(choosenGame (name Mass Effect 2))
=> 
	(assert (platform (name PC)))
	(assert (platform (name XBOX)))
	(assert (platform (name PS)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type RPG)))
	(assert (genre (type action)))
	(assert (genre (type science-fiction)))
	(assert (resultMessage (rule-id rule17) (message Mass Effect 2: PC/PS/XBOX single paid RPG action science-fiction)))
	(halt)
)

(defrule rule18
	(declare (salience 40))
	(platform (name PC|XBOX|PS))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type RPG))
	(genre (type action))
	(genre (type science-fiction))
=> 
	(assert (resultMessage (rule-id rule18) (message Mass Effect 2)))
	(halt)
)

(defrule rule19
	(declare (salience 40))
	(choosenGame (name God of War))
=> 
	(assert (platform (name PS)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type action)))
	(assert (genre (type adventure)))
	(assert (resultMessage (rule-id rule19) (message God of War: PS single paid action adventure)))
	(halt)
)

(defrule rule20
	(declare (salience 40))
	(platform (name PS))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type action))
	(genre (type adventure))
=> 
	(assert (resultMessage (rule-id rule20) (message God of War)))
	(halt)
)

(defrule rule21
	(declare (salience 40))
	(choosenGame (name Death Stranding))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type action)))
	(assert (genre (type open-world)))
	(assert (genre (type adventure)))
	(assert (genre (type science-fiction)))
	(assert (resultMessage (rule-id rule21) (message Death Stranding: PC/PS single paid open-world action adventure science-fiction)))
	(halt)
)

(defrule rule22
	(declare (salience 40))
	(platform (name PC|PS))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type action))
	(genre (type open-world))
	(genre (type adventure))
	(genre (type science-fiction))
=> 
	(assert (resultMessage (rule-id rule22) (message Death Stranding)))
	(halt)
)

(defrule rule23
	(declare (salience 40))
	(choosenGame (name Skyrim))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type RPG)))
	(assert (genre (type action)))
	(assert (genre (type open-world)))
	(assert (resultMessage (rule-id rule23) (message Skyrim: PC/PS/XBOX single paid open-world action RPG)))
	(halt)
)

(defrule rule24
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type RPG))
	(genre (type action))
	(genre (type open-world))
=> 
	(assert (resultMessage (rule-id rule24) (message Skyrim)))
	(halt)
)

(defrule rule25
	(declare (salience 40))
	(choosenGame (name Fallout 4))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type RPG)))
	(assert (genre (type action)))
	(assert (genre (type open-world)))
	(assert (resultMessage (rule-id rule25) (message Fallout 4: PC/PS/XBOX single paid open-world action RPG)))
	(halt)
)

(defrule rule26
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type RPG))
	(genre (type action))
	(genre (type open-world))
=> 
	(assert (resultMessage (rule-id rule26) (message Fallout 4)))
	(halt)
)

(defrule rule27
	(declare (salience 40))
	(choosenGame (name Fallout 76))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type RPG)))
	(assert (genre (type action)))
	(assert (genre (type open-world)))
	(assert (resultMessage (rule-id rule27) (message Fallout 76: PC/PS/XBOX multiplayer paid open-world action RPG)))
	(halt)
)

(defrule rule28
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type multiplayer))
	(paid_or_free (type paid))
	(genre (type RPG))
	(genre (type action))
	(genre (type open-world))
=> 
	(assert (resultMessage (rule-id rule28) (message Fallout 76)))
	(halt)
)

(defrule rule29
	(declare (salience 40))
	(choosenGame (name The Last of Us))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type survival)))
	(assert (genre (type stealth)))
	(assert (genre (type horror)))
	(assert (genre (type action)))
	(assert (genre (type adventure)))
	(assert (resultMessage (rule-id rule29) (message The Last of Us: PC/PS single paid survival stealth horror action adventure)))
	(halt)
)

(defrule rule30
	(declare (salience 40))
	(platform (name PC|PS))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type survival))
	(genre (type stealth))
	(genre (type horror))
	(genre (type action))
	(genre (type adventure))
=> 
	(assert (resultMessage (rule-id rule30) (message The Last of Us)))
	(halt)
)

(defrule rule31
	(declare (salience 40))
	(choosenGame (name Payday 2))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type shooter)))
	(assert (genre (type strategy)))
	(assert (resultMessage (rule-id rule31) (message Payday 2: PC/PS/XBOX multiplayer paid shooter strategy)))
	(halt)
)

(defrule rule32
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type multiplayer))
	(paid_or_free (type paid))
	(genre (type shooter))
	(genre (type strategy))
=> 
	(assert (resultMessage (rule-id rule32) (message Payday 2)))
	(halt)
)

(defrule rule33
	(declare (salience 40))
	(choosenGame (name Half-Life))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type shooter)))
	(assert (genre (type adventure)))
	(assert (genre (type science-fiction)))
	(assert (resultMessage (rule-id rule33) (message Half-Life: PC/PS/XBOX single paid shooter adventure science-fiction)))
	(halt)
)

(defrule rule34
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type shooter))
	(genre (type adventure))
	(genre (type science-fiction))
=> 
	(assert (resultMessage (rule-id rule34) (message Half-Life)))
	(halt)
)

(defrule rule35
	(declare (salience 40))
	(choosenGame (name Portal))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type puzzle)))
	(assert (genre (type science-fiction)))
	(assert (resultMessage (rule-id rule35) (message Portal: PC/PS/XBOX single paid puzzle science-fiction)))
	(halt)
)

(defrule rule36
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type puzzle))
	(genre (type science-fiction))
=> 
	(assert (resultMessage (rule-id rule36) (message Portal)))
	(halt)
)

(defrule rule37
	(declare (salience 40))
	(choosenGame (name Team Fortress 2))
=> 
	(assert (platform (name PC)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (paid_or_free (type free)))
	(assert (genre (type shooter)))
	(assert (resultMessage (rule-id rule37) (message Team Fortress 2: PC/XBOX multiplayer free shooter)))
	(halt)
)

(defrule rule38
	(declare (salience 40))
	(platform (name PC|XBOX))
	(single_or_multiplayer (type multiplayer))
	(paid_or_free (type free))
	(genre (type shooter))
=> 
	(assert (resultMessage (rule-id rule38) (message Team Fortress 2)))
	(halt)
)

(defrule rule39
	(declare (salience 40))
	(choosenGame (name Garry's Mod))
=> 
	(assert (platform (name PC)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type sandbox)))
	(assert (resultMessage (rule-id rule39) (message Garry's Mod: PC single/multiplayer paid sandbox)))
	(halt)
)

(defrule rule40
	(declare (salience 40))
	(platform (name PC))
	(single_or_multiplayer (type multiplayer|single))
	(paid_or_free (type paid))
	(genre (type sandbox))
=> 
	(assert (resultMessage (rule-id rule40) (message Garry's Mod)))
	(halt)
)

(defrule rule41
	(declare (salience 40))
	(choosenGame (name Rust))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type survival)))
	(assert (genre (type sandbox)))
	(assert (genre (type open-world)))
	(assert (resultMessage (rule-id rule41) (message Rust: PC/PS/XBOX single/multiplayer paid survival sandbox open-world)))
	(halt)
)

(defrule rule42
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type multiplayer|single))
	(paid_or_free (type paid))
	(genre (type survival))
	(genre (type sandbox))
	(genre (type open-world))
=> 
	(assert (resultMessage (rule-id rule42) (message Rust)))
	(halt)
)

(defrule rule43
	(declare (salience 40))
	(choosenGame (name GTA 5))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type adventure)))
	(assert (genre (type action)))
	(assert (genre (type open-world)))
	(assert (resultMessage (rule-id rule43) (message GTA 5: PC/PS/XBOX single/multiplayer paid adventure action open-world)))
	(halt)
)

(defrule rule44
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type multiplayer|single))
	(paid_or_free (type paid))
	(genre (type adventure))
	(genre (type action))
	(genre (type open-world))
=> 
	(assert (resultMessage (rule-id rule44) (message GTA 5)))
	(halt)
)

(defrule rule45
	(declare (salience 40))
	(choosenGame (name Max Payne 3))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type shooter)))
	(assert (genre (type action)))
	(assert (resultMessage (rule-id rule45) (message Max Payne 3: PC/PS/XBOX single/multiplayer paid shooter action)))
	(halt)
)

(defrule rule46
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type multiplayer|single))
	(paid_or_free (type paid))
	(genre (type shooter))
	(genre (type action))
=> 
	(assert (resultMessage (rule-id rule46) (message Max Payne 3)))
	(halt)
)

(defrule rule47
	(declare (salience 40))
	(choosenGame (name Dota 2))
=> 
	(assert (platform (name PC)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (paid_or_free (type free)))
	(assert (genre (type strategy)))
	(assert (genre (type RPG)))
	(assert (resultMessage (rule-id rule47) (message Dota 2: PC multiplayer free strategy RPG)))
	(halt)
)

(defrule rule48
	(declare (salience 40))
	(platform (name PC))
	(single_or_multiplayer (type multiplayer))
	(paid_or_free (type free))
	(genre (type strategy))
	(genre (type RPG))
=> 
	(assert (resultMessage (rule-id rule48) (message Dota 2)))
	(halt)
)

(defrule rule49
	(declare (salience 40))
	(choosenGame (name Warcraft 3))
=> 
	(assert (platform (name PC)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type free)))
	(assert (genre (type strategy)))
	(assert (genre (type RPG)))
	(assert (resultMessage (rule-id rule49) (message Warcraft 3: PC single/multiplayer free strategy RPG)))
	(halt)
)

(defrule rule50
	(declare (salience 40))
	(platform (name PC))
	(single_or_multiplayer (type single|multiplayer))
	(paid_or_free (type free))
	(genre (type strategy))
	(genre (type RPG))
=> 
	(assert (resultMessage (rule-id rule50) (message Warcraft 3)))
	(halt)
)

(defrule rule51
	(declare (salience 40))
	(choosenGame (name Little Nightmares))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type puzzle)))
	(assert (genre (type stealth)))
	(assert (genre (type horror)))
	(assert (resultMessage (rule-id rule51) (message Little Nightmares: PC/PS/XBOX single paid puzzle stealth horror)))
	(halt)
)

(defrule rule52
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type puzzle))
	(genre (type stealth))
	(genre (type horror))
=> 
	(assert (resultMessage (rule-id rule52) (message Little Nightmares)))
	(halt)
)

(defrule rule53
	(declare (salience 40))
	(choosenGame (name Formula 1 2023))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type sport)))
	(assert (genre (type race)))
	(assert (resultMessage (rule-id rule53) (message Formula 1 2023: PC/PS/XBOX single/multiplayer paid sport race)))
	(halt)
)

(defrule rule54
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type single|multiplayer))
	(paid_or_free (type paid))
	(genre (type sport))
	(genre (type race))
=> 
	(assert (resultMessage (rule-id rule54) (message Formula 1 2023)))
	(halt)
)

(defrule rule55
	(declare (salience 40))
	(choosenGame (name NHL 24))
=> 
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type sport)))
	(assert (resultMessage (rule-id rule55) (message NHL 24: PS/XBOX single/multiplayer paid sport)))
	(halt)
)

(defrule rule56
	(declare (salience 40))
	(platform (name PS|XBOX))
	(single_or_multiplayer (type single|multiplayer))
	(paid_or_free (type paid))
	(genre (type sport))
=> 
	(assert (resultMessage (rule-id rule56) (message NHL 24)))
	(halt)
)

(defrule rule57
	(declare (salience 40))
	(choosenGame (name HITMAN 3))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type shooter)))
	(assert (genre (type stealth)))
	(assert (resultMessage (rule-id rule57) (message HITMAN 3: PC/PS/XBOX single paid shooter stealth)))
	(halt)
)

(defrule rule58
	(declare (salience 40))
	(choosenGame (name HITMAN 3))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type shooter))
	(genre (type stealth))
=> 
	(assert (resultMessage (rule-id rule58) (message HITMAN 3)))
	(halt)
)

(defrule rule59
	(declare (salience 40))
	(choosenGame (name The Forest))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type survival)))
	(assert (genre (type horror)))
	(assert (genre (type open-world)))
	(assert (resultMessage (rule-id rule59) (message The Forest: PC/PS single/multiplayer paid survival horror open-world)))
	(halt)
)

(defrule rule60
	(declare (salience 40))
	(platform (name PC|PS))
	(single_or_multiplayer (type single|multiplayer))
	(paid_or_free (type paid))
	(genre (type survival))
	(genre (type horror))
	(genre (type open-world))
=> 
	(assert (resultMessage (rule-id rule60) (message The Forest)))
	(halt)
)

(defrule rule61
	(declare (salience 40))
	(choosenGame (name Teardown))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type single)))
	(assert (single_or_multiplayer (type multiplayer)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type sandbox)))
	(assert (resultMessage (rule-id rule61) (message Teardown: PC/PS/XBOX single/multiplayer paid sandbox)))
	(halt)
)

(defrule rule62
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type single|multiplayer))
	(paid_or_free (type paid))
	(genre (type sandbox))
=> 
	(assert (resultMessage (rule-id rule62) (message Teardown)))
	(halt)
)

(defrule rule63
	(declare (salience 40))
	(choosenGame (name Witcher 3))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type RPG)))
	(assert (genre (type action)))
	(assert (genre (type open-world)))
	(assert (genre (type adventure)))
	(assert (resultMessage (rule-id rule63) (message Witcher 3: PC/PS/XBOX single paid RPG action open-world adventure)))
	(halt)
)

(defrule rule64
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type RPG))
	(genre (type action))
	(genre (type open-world))
	(genre (type adventure))
=> 
	(assert (resultMessage (rule-id rule64) (message Witcher 3)))
	(halt)
)

(defrule rule65
	(declare (salience 40))
	(choosenGame (name Bloodborne))
=> 
	(assert (platform (name PS)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type RPG)))
	(assert (genre (type stealth)))
	(assert (genre (type horror)))
	(assert (genre (type action)))
	(assert (resultMessage (rule-id rule65) (message Bloodborne: PS single paid RPG action stealth horror)))
	(halt)
)

(defrule rule66
	(declare (salience 40))
	(platform (name PS))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type RPG))
	(genre (type stealth))
	(genre (type horror))
	(genre (type action))
=> 
	(assert (resultMessage (rule-id rule66) (message Bloodborne)))
	(halt)
)

(defrule rule67
	(declare (salience 40))
	(choosenGame (name Atomic Heart))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type RPG)))
	(assert (genre (type action)))
	(assert (genre (type shooter)))
	(assert (genre (type adventure)))
	(assert (genre (type science-fiction)))
	(assert (resultMessage (rule-id rule67) (message Atomic Heart: PC/PS/XBOX single paid RPG action shooter adventure science-fiction)))
	(halt)
)

(defrule rule68
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type RPG))
	(genre (type action))
	(genre (type shooter))
	(genre (type adventure))
	(genre (type science-fiction))
=> 
	(assert (resultMessage (rule-id rule68) (message Atomic Heart)))
	(halt)
)

(defrule rule69
	(declare (salience 40))
	(choosenGame (name Cyberpunk 2077))
=> 
	(assert (platform (name PC)))
	(assert (platform (name PS)))
	(assert (platform (name XBOX)))
	(assert (single_or_multiplayer (type single)))
	(assert (paid_or_free (type paid)))
	(assert (genre (type RPG)))
	(assert (genre (type action)))
	(assert (genre (type shooter)))
	(assert (genre (type adventure)))
	(assert (genre (type science-fiction)))
	(assert (genre (type open-world)))
	(assert (resultMessage (rule-id rule69) (message Cyberpunk 2077: PC/PS/XBOX single paid RPG action shooter adventure science-fiction open-world)))
	(halt)
)

(defrule rule70
	(declare (salience 40))
	(platform (name PC|PS|XBOX))
	(single_or_multiplayer (type single))
	(paid_or_free (type paid))
	(genre (type RPG))
	(genre (type action))
	(genre (type shooter))
	(genre (type adventure))
	(genre (type science-fiction))
	(genre (type open-world))
=> 
	(assert (resultMessage (rule-id rule70) (message Cyberpunk 2077)))
	(halt)
)