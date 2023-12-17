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

(deftemplate select-task ;шаблон факта-посредника для выбора специализации героя
	(slot hero-name)
	(slot task)
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
)

(defrule set-output-and-halt
	(declare (salience 99))
	?current-message <- (sendmessagehalt ?new-msg)
	?proxy <- (ioproxy (messages $?msg-list))
	=>
	(modify ?proxy (messages ?new-msg))
	(retract ?current-message)
	(halt)
)

(deftemplate role (slot type))
(deftemplate attack (slot type))
(deftemplate line (slot type))
(deftemplate hero (slot name))
(deftemplate task (slot type))
(deftemplate teamReady (slot type))

(defrule set-input-hero-and-halt
	(declare (salience 98))
	?current-message <- (addhero ?new-answer)
	;?proxy <- (ioproxy (answers $?msg-list))
	=>

	(assert (hero (name ?new-answer)))
	(retract ?current-message)
	(halt)
)

(defrule set-input-task-and-halt
	(declare (salience 98))
	?current-message <- (addtask ?hero)
	?proxy <- (select-task (hero-name ?hero) (task ?hero-task))
	=>
	(assert (task (type ?hero-task)))
	(retract ?current-message)
	(retract ?proxy)
	(assert (sendmessagehalt "Специализация добавлена"))
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

(defrule rule1
	(declare (salience 1))
	(hero (name Juggernaut))
=>
	(assert (select-task (hero-name Juggernaut)))
	(assert (attack (type MeleeCarry)))
	(assert (sendmessagehalt "Добавлен герой Juggernaut, выберите для него специализацию:"))
)

(defrule rule2
	(declare (salience 1))
	(hero (name Invoker))
=>
	(assert (select-task (hero-name Invoker)))
	(assert (attack (type RangeMid)))
	(assert (sendmessagehalt "Добавлен герой Invoker, выберите для него специализацию:"))
)


(defrule rule3
	(declare (salience 1))
	(hero (name Pudge))
=>
	(assert (select-task (hero-name Pudge)))
	(assert (attack (type MeleeHard)))
	(assert (sendmessagehalt "Добавлен герой Pudge, выберите для него специализацию:"))
)

(defrule rule4
	(declare (salience 1))
	(hero (name Rubik))
=>
	(assert (select-task (hero-name Rubik)))
	(assert (attack (type RangeSoftSupport)))
	(assert (sendmessagehalt "Добавлен герой Rubik, выберите для него специализацию:"))
)

(defrule rule5
	(declare (salience 1))
	(hero (name Dazzle))
=>
	(assert (select-task (hero-name Dazzle)))
	(assert (attack (type RangeSupport)))
	(assert (sendmessagehalt "Добавлен герой Dazzle, выберите для него специализацию:"))
)


(defrule rule6
	(declare (salience 1))
	(task (type Push))
=>
	(assert (role (type Carry)))
	(assert (sendmessagehalt "Роль керри найдена"))
)

(defrule rule7
	(declare (salience 1))
	(task (type Burst))
=>
	(assert (role (type Mid)))
	(assert (sendmessagehalt "Роль мид найдена"))
)

(defrule rule8
	(declare (salience 1))
	(task (type Tank))
=>
	(assert (role (type Hard)))
	(assert (sendmessagehalt "Роль хард найдена"))
)

(defrule rule9
	(declare (salience 1))
	(task (type Initiator))
=>
	(assert (role (type SoftSupport)))
	(assert (sendmessagehalt "Роль поддержки найдена"))
)

(defrule rule10
	(declare (salience 1))
	(task (type Control))
=>
	(assert (role (type Support)))
	(assert (sendmessagehalt "Роль полнойподдержки найдена"))
)

(defrule rule11
	(declare (salience 1))
	(role (type Carry))
	(role (type Support))
	(attack (type MeleeCarry))
	(attack (type RangeSupport))
=>
	(assert (line (type EzLine)))
	(assert (sendmessagehalt "Легкая линия готова"))
)

(defrule rule12
	(declare (salience 1))
	(role (type Hard))
	(role (type SoftSupport))
	(attack (type MeleeHard))
	(attack (type RangeSoftSupport))
=>
	(assert (line (type HardLine)))
	(assert (sendmessagehalt "Тяжелая линия готова"))
)


(defrule rule13
	(declare (salience 1))
	;(line (type MidLine))
	(line (type EzLine))
	(line (type HardLine))
=>
	(assert (teamReady (type Ready)))
	(assert (sendmessagehalt "Команда готова"))
)

(defrule rule14
	(declare (salience 1))
	(hero (name Lifestealer))
=>
	(assert (select-task (hero-name Lifestealer)))
	(assert (attack (type MeleeCarry)))
	(assert (sendmessagehalt "Добавлен герой Lifestealer, выберите для него специализацию:"))
)

(defrule rule15
	(declare (salience 1))
	(hero (name Lina))
=>
	(assert (select-task (hero-name Lina)))
	(assert (attack (type RangeMid)))
	(assert (sendmessagehalt "Добавлен герой Lina, выберите для него специализацию:"))
)


(defrule rule16
	(declare (salience 1))
	(hero (name Bristleback))
=>
	(assert (select-task (hero-name Bristleback)))
	(assert (attack (type MeleeHard)))
	(assert (sendmessagehalt "Добавлен герой Bristleback, выберите для него специализацию:"))
)

(defrule rule17
	(declare (salience 1))
	(hero (name Bane))
=>
	(assert (select-task (hero-name Bane)))
	(assert (attack (type RangeSoftSupport)))
	(assert (sendmessagehalt "Добавлен герой Bane, выберите для него специализацию:"))
)

(defrule rule18
	(declare (salience 1))
	(hero (name Lich))
=>
	(assert (select-task (hero-name Lich)))
	(assert (attack (type RangeSupport)))
	(assert (sendmessagehalt "Добавлен герой Lich, выберите для него специализацию:"))
)

(defrule rule19
	(declare (salience 1))
	(task (type RoshanDamage))
=>
	(assert (role (type Carry)))
	(assert (sendmessagehalt "Роль керри найдена"))
)