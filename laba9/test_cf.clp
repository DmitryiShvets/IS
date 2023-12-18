(deftemplate role (slot type) (slot cf (type FLOAT) (default 0.0)))
(deftemplate attack (slot type) (slot cf (type FLOAT) (default -1.0)))
(deftemplate line (slot type) (slot cf (type FLOAT) (default 0.0)))
(deftemplate hero (slot name) (slot cf (type FLOAT) (default 0.0)))
(deftemplate task (slot type) (slot cf (type FLOAT) (default 0.0)))
(deftemplate threshold (slot cf (type FLOAT) (default 0.0)))
(deftemplate teamReady (slot type) (slot cf (type FLOAT) (default 0.0)))

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

(defrule set-input-hero-and-halt_modify
	(declare (salience 99))
	?current-message <- (addhero ?new-answer ?new_cf)
	?existed_hero <- (hero (name ?new-answer) (cf ?old_cf))
	=>
	(modify ?existed_hero (cf (+ ?new_cf ?old_cf)))
	(retract ?current-message)
	(halt)
)

(defrule set-input-hero-and-halt
	(declare (salience 98))
	?current-message <- (addhero ?new-answer ?new_cf)
	;?proxy <- (ioproxy (answers $?msg-list))
	=>
	(assert (hero (name ?new-answer) (cf ?new_cf)))
	(retract ?current-message)
	(halt)
)

; Сделать textBox с коэффициентами и передавать в select-task
; Реализовать коэффициент полезности героя(чтобы 5 героев были с коэффициентами полезности в диапазоне от -0.9 до 0.9)
; Реализовать коэффициент полезности для роли героя(от -0.9 до 0.9)
; Сделать правило, которое будет учитывать, чтобы у нас было 5 ролей
; Написать правило, которое работает хотя бы с одним героем(строит вывод)

; input_hero(text_box_hero_cf) -> hero -> select_task(text_box_task_cf) -> add_task -> mult(hero_cf, task_cf) = коэффииент полезности героя от роли

(defrule set-input-task-and-halt
	(declare (salience 98))
	?current-message <- (addtask ?hero ?cf_task)
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
    ?trashold <- (threshold (cf ?rat))
    (test (< ?rat 3.6)) 
    =>
	(assert (hero (name ?new-answer)))
    (assert (sendmessagehalt ?rat))
)

(deffunction cmb (?a ?b)
    (if (and (>= ?a 0) (>= ?b 0)) then
        (- (+ ?a ?b) (* ?a ?b))
        else
            (if (and (< ?a 0) (< ?b 0)) then
            (+ (+ ?a ?b) (* ?a ?b))
            else
                (/ (+ ?a ?b) (- 1 (min (abs ?a) (abs ?b))))
        )
    )
)

(defrule rule1
	(declare (salience 1))
	(hero (name Juggernaut) (cf ?rat))
=>
	(assert (select-task (hero-name Juggernaut)))
	(assert (attack (type MeleeCarry) (cf (+ ?rat 0.2))))
	(assert (sendmessagehalt "Добавлен герой Juggernaut, выберите для него специализацию:"))
)



(defrule rule14
	(declare (salience 1))
	(hero (name Lifestealer) (cf ?rat))
=>
	(assert (select-task (hero-name Lifestealer)))
	(assert (attack (type MeleeCarry) (cf (* ?rat 1.0))))
	(assert (sendmessagehalt "Добавлен герой Lifestealer, выберите для него специализацию:"))
)

(defrule rule114
	(declare (salience 1))
	(hero (name Lifestealer) (cf ?rat))
=>
	(assert (select-task (hero-name Lifestealer)))
	(assert (attack (type MeleeCarry) (cf (* ?rat 1.0))))
	(assert (sendmessagehalt "Добавлен герой Lifestealer, выберите для него специализацию:"))
)

(defrule rule2
	(declare (salience 1))
	(hero (name Invoker) (cf ?rat))
=>
	(assert (select-task (hero-name Invoker)))
	(assert (attack (type RangeMid) (cf (* ?rat 1.0))))
	(assert (sendmessagehalt "Добавлен герой Invoker, выберите для него специализацию:"))
)

(defrule rule3
	(declare (salience 1))
	(hero (name Pudge) (cf ?rat))
=>
	(assert (select-task (hero-name Pudge)))
	(assert (attack (type MeleeHard) (cf (* ?rat 1.0))))
	(assert (sendmessagehalt "Добавлен герой Pudge, выберите для него специализацию:"))
)

(defrule rule4
	(declare (salience 1))
	(hero (name Rubik) (cf ?rat))
=>
	(assert (select-task (hero-name Rubik)))
	(assert (attack (type RangeSoftSupport) (cf (* ?rat 1.0))))
	(assert (sendmessagehalt "Добавлен герой Rubik, выберите для него специализацию:"))
)

(defrule rule5
	(declare (salience 1))
	(hero (name Dazzle) (cf ?rat))
=>
	(assert (select-task (hero-name Dazzle)))
	(assert (attack (type RangeSupport) (cf (* ?rat 1.0))))
	(assert (sendmessagehalt "Добавлен герой Dazzle, выберите для него специализацию:"))
)