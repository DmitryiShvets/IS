(deftemplate role (slot type))
(deftemplate attack (slot type))
(deftemplate line (slot type))
(deftemplate hero (slot name))
(deftemplate task (slot type))
(deftemplate confidence_factor (slot value))
(deftemplate threshold (slot value (type FLOAT) (default 0.0)))
(deftemplate teamReady (slot type))

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
	(declare (salience 98))
	?current-message <- (sendmessagehalt ?new-msg)
	?proxy <- (ioproxy (messages $?msg-list))
	=>
	(modify ?proxy (messages ?new-msg))
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


(defrule set-input-hero-and
    (declare (salience 98))
    ?current-message <- (addhero ?new-answer)
    ?trashold <- (threshold (value ?rat))
    (test (< ?rat 3.6)) 
    =>
    (assert (sendmessagehalt ?rat))
)

(defrule rule1
	(declare (salience 1))
	(hero (name Juggernaut))
=>
	(assert (select-task (hero-name Juggernaut)))
	(assert (attack (type MeleeCarry)))
	(assert (sendmessagehalt "Добавлен герой Juggernaut, выберите для него специализацию:"))
)