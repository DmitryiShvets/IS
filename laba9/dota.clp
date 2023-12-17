;========================================================================
; Этот блок реализует логику обмена информацией с графической оболочкой,
; а также механизм остановки и повторного пуска машины вывода
; Русский текст в комментариях разрешён!

(deftemplate ioproxy  ; шаблон факта-посредника для обмена информацией с GUI
	(slot fact-id)        ; теоретически тут id факта для изменения
	(multislot answers)   ; возможные ответы
	(multislot messages)  ; исходящие сообщения
	(slot reaction)       ; возможные ответы пользователя
	(slot value)          ; выбор пользователя
	(slot restore)        ; забыл зачем это поле
)

; Собственно экземпляр факта ioproxy
(deffacts proxy-fact
	(ioproxy
		(fact-id 0112) ; это поле пока что не задействовано
		(value none)   ; значение пустое
		(messages)     ; мультислот messages изначально пуст
	)
)

(defrule clear-messages
	(declare (salience 90))
	?clear-msg-flg <- (clearmessage)
	?proxy <- (ioproxy)
	=>
	(modify ?proxy (messages))
	(retract ?clear-msg-flg)
	(printout t "Messages cleared ..." crlf)	
)

(defrule set-output-and-halt
	(declare (salience 99))
	?current-message <- (sendmessagehalt ?new-msg)
	?proxy <- (ioproxy (messages $?msg-list))
	=>
	(printout t "Message set : " ?new-msg " ... halting ..." crlf)
	(modify ?proxy (messages ?new-msg))
	(retract ?current-message)
	(halt)
)

;(defrule append-output-and-halt
;	//  Аналогичен предыдущему, но с добавлением сообщения, а не с заменой
;)

;(defrule set-output-and-proceed
;	//  Аналогичен предыдущему, но с установкой сообщения и продолжением работы (извлекая факт с текущим сообщением)
;)

;(defrule append-output-and-proceed
;	//  По аналогии
;)

;======================================================================================

(deftemplate role (slot type))
(deftemplate attack (slot type))
(deftemplate line (slot type))
(deftemplate task (slot type))
(deftemplate hero (slot name))

(deftemplate teamReady (slot type))
(deftemplate resultMessage
	(multislot message)
	(multislot answer)
	(slot rule-id))

(deffacts roles
	(role (type Carry))
	(role (type Mid))
	(role (type Hard))
	(role (type SoftSupport))
	(role (type Support))
)

(deffacts attacks
	(role (type MeleeCarry))
	(role (type RangeCarry))
	(role (type MeleeMid))
	(role (type RangeMid))
	(role (type MeleeHard))
	(role (type RangeHard))
	(role (type MeleeSoftSupport))
	(role (type RangeSoftSupport))
	(role (type MeleeSupport))
	(role (type RangeSupport))
)

(deffacts lines
	(role (type EzLine))
	(role (type HardLine))
	(role (type MidLine))
)

(deffacts tasks
	(role (type Push))
	(role (type Burst))
	(role (type Tank))
	(role (type Initiator))
	(role (type Control))
)

(deffacts heroes
	(hero (name Juggernaut))
	(hero (name Phantom_Assasin))
	(hero (name Lifestealer))
	(hero (name Drow_Ranger))
	(hero (name Medusa))
	(hero (name Luna))
	(hero (name Dragon_Knight))
	(hero (name Ember_Spirit))
	(hero (name Meepo))
	(hero (name Sniper))
	(hero (name Lina))
	(hero (name Invoker))
	(hero (name Pudge))
	(hero (name Bristleback))
	(hero (name Centaur_Warrunner))
	(hero (name Necrophos))
	(hero (name Batrider))
	(hero (name Razor))
	(hero (name Spirit_Breaker))
	(hero (name Earthshaker))
	(hero (name Tiny))
	(hero (name Rubik))
	(hero (name Dark_Willow))
	(hero (name Bane))
	(hero (name Ogre_Magi))
	(hero (name Treant_Protector))
	(hero (name Abaddon))
	(hero (name Crystal_Maiden))
	(hero (name Shadow_Shaman))
	(hero (name Dazzle))
)

(defrule no-result
	(declare (salience 1))
	=>
	(assert (resultMessage (rule-id no-result) (message No Match)))
)

(defrule rule1
	(declare (salience 1))
	(hero (name Juggernaut))
=>
	(assert (task (type Push)))
	(assert (attack (type MeleeCarry)))
	(assert (resultMessage (rule-id rule1)(message Juggernaut: Push MeleeCarry)))
	(assert (sendmessagehalt "Если герой Juggernaut, то Push MeleeCarry"))
	;(halt)
)

(defrule rule2
	(declare (salience 1))
	(hero (name Invoker))
=>
	(assert (task (type Burst)))
	(assert (attack (type RangeMid)))
	;(assert (ioproxy (fact-id rule2) (value none) (messages Invoker: Burst RangeMid)))
	(assert (sendmessagehalt "Если герой Invoker, то Burst RangeMid"))

	;(halt)
)

(defrule rule3
	(declare (salience 1))
	(hero (name Pudge))
=>
	(assert (task (type Tank)))
	(assert (attack (type MeleeHard)))
	(assert (resultMessage (rule-id rule3) (message Pudge: Tank MeleeHard)))
	(assert (sendmessagehalt "Если герой Pudge, то Tank MeleeHard"))
	;(halt)
)

(defrule rule4
	(declare (salience 1))
	(hero (name Rubik))
=>
	(assert (task (type Initiator)))
	(assert (attack (type RangeSoftSupport)))
	(assert (resultMessage (rule-id rule4) (message Rubik: Initiator RangeSoftSupport)))
	(assert (sendmessagehalt "Если герой Rubik, то Initiator RangeSoftSupport"))

	;(halt)
)

(defrule rule5
	(declare (salience 1))
	(hero (name Dazzle))
=>
	(assert (task (type Control)))
	(assert (attack (type RangeSupport)))
	(assert (sendmessagehalt "Если герой Dazzle, то Control RangeSupport"))
	;(assert (resultMessage (rule-id rule5) (message Dazzle: Control RangeSupport)))
	;(halt)
)

(defrule rule6
	(declare (salience 1))
	(task (type Push))
=>
	(assert (role (type Carry)))
	(assert (sendmessagehalt "Если Push, то Carry"))
	;(assert (resultMessage (rule-id rule6) (message Push: Carry)))
	;(halt)
)

(defrule rule7
	(declare (salience 1))
	(task (type Burst))
=>
	(assert (role (type Mid)))
	(assert (sendmessagehalt "Если Burst, то Mid"))
	;(assert (resultMessage (rule-id rule7) (message Burst: Mid)))
	;(halt)
)

(defrule rule8
	(declare (salience 1))
	(task (type Tank))
=>
	(assert (role (type Hard)))
	(assert (sendmessagehalt "Если Tank, то Hard"))
	;(assert (resultMessage (rule-id rule8) (message Tank: Hard)))
	;(halt)
)

(defrule rule9
	(declare (salience 1))
	(task (type Initiator))
=>
	(assert (role (type SoftSupport)))
	(assert (sendmessagehalt "Если Initiator, то SoftSupport"))
	;(assert (resultMessage (rule-id rule9) (message Initiator: SoftSupport)))
	;(halt)
)

(defrule rule10
	(declare (salience 1))
	(task (type Control))
=>
	(assert (role (type Support)))
	(assert (sendmessagehalt "Если Control, то Support"))
	;(assert (resultMessage (rule-id rule10) (message Control: Support)))
	;(halt)
)

(defrule rule11
	(declare (salience 1))
	(role (type Carry))
	(role (type Support))
	(attack (type MeleeCarry))
	(attack (type RangeSupport))
=>
	(assert (line (type EzLine)))
	(assert (sendmessagehalt "Если Carry Support MeleeCarry RangeSupport, то EzLine"))
	;(assert (resultMessage (rule-id rule11) (message Carry Support MeleeCarry RangeSupport: EzLine)))
	;(halt)
)

(defrule rule12
	(declare (salience 1))
	(role (type Hard))
	(role (type SoftSupport))
	(attack (type MeleeHard))
	(attack (type RangeSoftSupport))
=>
	(assert (line (type HardLine)))
	(assert (sendmessagehalt "Если Hard SoftSupport MeleeHard RangeSoftSupport, то HardLine"))
	;(assert (resultMessage (rule-id rule12) (message Hard SoftSupport MeleeHard RangeSoftSupport: HardLine)))
	;(halt)
)

(defrule rule13
	(declare (salience 1))
	(role (type Mid))
	(attack (type RangeMid))
=>
	(assert (line (type MidLine)))
	(assert (sendmessagehalt "Если Mid RangeMid, то MidLine"))
	;(assert (resultMessage (rule-id rule13) (message Mid RangeMid: MidLine)))
	;(halt)
)

(defrule rule14
	(declare (salience 1))
	(line (type EzLine))
	(line (type HardLine))
	(line (type MidLine))
=>
	(assert (teamReady (type Ready)))
	(assert (sendmessagehalt "Если EzLine HardLine MidLine, то Ready"))
	;(assert (resultMessage (rule-id rule14) (message EzLine HardLine MidLine: Ready)))
	;(halt)
)
