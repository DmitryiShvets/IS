(deftemplate role (slot type) (slot cf (type FLOAT) (default 0.0)))
(deftemplate attack (slot type) (slot cf (type FLOAT) (default 0.0)))
(deftemplate line (slot type) (slot cf (type FLOAT) (default 0.0)))
(deftemplate hero (slot name) (slot cf (type FLOAT) (default 0.0)))
(deftemplate task (slot type) (slot cf (type FLOAT) (default 0.0)))
(deftemplate threshold (slot cf (type FLOAT) (default 0.0)))
(deftemplate teamReady (slot type) (slot cf (type FLOAT) (default 0.0)))
(deftemplate countHeroes (slot value (type INTEGER) (default 0)))
(deftemplate winrate (slot value (type FLOAT) (default 0.0)))

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

(deftemplate select-task ;шаблон факта-посредника для выбора специализации героя
	(slot hero-name)
	(slot task)
)

(deffacts init_facts
    (task (type Push) (cf 0.0))
    (task (type Burst) (cf 0.0))
    (task (type Tank) (cf 0.0))
    (task (type Initiator) (cf 0.0))
    (task (type Control) (cf 0.0))
	(countHeroes (value 0))
	(winrate (value 0.0))
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
	?existed_trashold <- (threshold (cf ?trash))
	(test (> ?new_cf ?trash))
	=>
	(assert (hero (name ?new-answer) (cf ?new_cf)))
	(retract ?current-message)
	(halt)
)

; Сделать textBox с коэффициентами и передавать в select-task
; Реализовать коэффициент полезности героя(чтобы 5 героев были с коэффициентами полезности в диапазоне от -0.9 до 0.9)
; Реализовать коэффициент полезности для роли героя(от -0.9 до 0.9)
; Написать правило, которое работает хотя бы с одним героем(строит вывод)

; input_hero(text_box_hero_cf) -> hero -> select_task(text_box_task_cf) -> add_task -> mult(hero_cf, task_cf) = коэффииент полезности героя от роли

(defrule set-input-task-and-halt
	(declare (salience 98))
	?current-message <- (addtask ?hero ?cf_task)
	?proxy <- (select-task (hero-name ?hero) (task ?hero-task))
	?existed_hero <- (hero (name ?hero) (cf ?old_hero_cf))
	?existed_task <- (task (type ?hero-task) (cf ?old_cf))
	?existed_heroes <- (countHeroes (value ?count))
	?existed_trashold <- (threshold (cf ?trash))
	(test (> (cmb (* ?cf_task ?old_hero_cf) ?old_cf) ?trash))
	=>
	(modify ?existed_task (cf (cmb (* ?cf_task ?old_hero_cf) ?old_cf)))
	(modify ?existed_heroes (value (+ ?count 1)))
	(retract ?current-message)
	(retract ?proxy)
	(assert (sendmessagehalt (str-cat "Специализация добавлена "  (cmb (* ?cf_task ?old_hero_cf) ?old_cf))))
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
	(hero (name Juggernaut) (cf ?rat))
=>
	(assert (select-task (hero-name Juggernaut)))
	(assert (attack (type MeleeCarry)))
	(assert (sendmessagehalt "Добавлен герой Juggernaut, выберите для него специализацию:"))
)

(defrule rule2
	(declare (salience 1))
	(hero (name Invoker) (cf ?rat))
=>
	(assert (select-task (hero-name Invoker)))
	(assert (attack (type RangeMid)))
	(assert (sendmessagehalt "Добавлен герой Invoker, выберите для него специализацию:"))
)

(defrule rule3
	(declare (salience 1))
	(hero (name Pudge) (cf ?rat))
=>
	(assert (select-task (hero-name Pudge)))
	(assert (attack (type MeleeHard)))
	(assert (sendmessagehalt "Добавлен герой Pudge, выберите для него специализацию:"))
)

(defrule rule4
	(declare (salience 1))
	(hero (name Rubik) (cf ?rat))
=>
	(assert (select-task (hero-name Rubik)))
	(assert (attack (type RangeSoftSupport)))
	(assert (sendmessagehalt "Добавлен герой Rubik, выберите для него специализацию:"))
)

(defrule rule5
	(declare (salience 1))
	(hero (name Dazzle) (cf ?rat))
=>
	(assert (select-task (hero-name Dazzle)))
	(assert (attack (type RangeSupport)))
	(assert (sendmessagehalt "Добавлен герой Dazzle, выберите для него специализацию:"))
)

(defrule rule6
	(declare (salience 1))
	(hero (name Lifestealer))
=>
	(assert (select-task (hero-name Lifestealer)))
	(assert (attack (type MeleeCarry)))
	(assert (sendmessagehalt "Добавлен герой Lifestealer, выберите для него специализацию:"))
)

(defrule rule7
	(declare (salience 1))
	(hero (name Lina))
=>
	(assert (select-task (hero-name Lina)))
	(assert (attack (type RangeMid)))
	(assert (sendmessagehalt "Добавлен герой Lina, выберите для него специализацию:"))
)

(defrule rule8
	(declare (salience 1))
	(hero (name Bristleback))
=>
	(assert (select-task (hero-name Bristleback)))
	(assert (attack (type MeleeHard)))
	(assert (sendmessagehalt "Добавлен герой Bristleback, выберите для него специализацию:"))
)

(defrule rule9
	(declare (salience 1))
	(hero (name Bane))
=>
	(assert (select-task (hero-name Bane)))
	(assert (attack (type RangeSoftSupport)))
	(assert (sendmessagehalt "Добавлен герой Bane, выберите для него специализацию:"))
)

(defrule rule10
	(declare (salience 1))
	(hero (name Lich))
=>
	(assert (select-task (hero-name Lich)))
	(assert (attack (type RangeSupport)))
	(assert (sendmessagehalt "Добавлен герой Lich, выберите для него специализацию:"))
)


(defrule rule11
	(declare (salience 1))
	?existed_heroes <- (countHeroes (value 1))
	?existed_winrate <- (winrate (value ?wr))
	?existed_push <- (task (type Push) (cf ?cf_push))
	?existed_burst <- (task (type Burst) (cf ?cf_burst))
	?existed_tank <- (task (type Tank) (cf ?cf_tank))
	?existed_initiator <- (task (type Initiator) (cf ?cf_initiator))
	?existed_control <- (task (type Control) (cf ?cf_control))
=>
	(modify ?existed_winrate (value (cmb (cmb ?cf_push (cmb ?cf_burst (cmb ?cf_tank (cmb ?cf_initiator ?cf_control)))) -0.8)))
	(assert (sendmessagehalt (str-cat "С командой из 1-го героев я даю вам winrate(в %):" (* (cmb (cmb ?cf_push (cmb ?cf_burst (cmb ?cf_tank (cmb ?cf_initiator ?cf_control)))) -0.8) 100))))
)

(defrule rule12
	(declare (salience 1))
	?existed_heroes <- (countHeroes (value 2))
	?existed_winrate <- (winrate (value ?wr))
	?existed_push <- (task (type Push) (cf ?cf_push))
	?existed_burst <- (task (type Burst) (cf ?cf_burst))
	?existed_tank <- (task (type Tank) (cf ?cf_tank))
	?existed_initiator <- (task (type Initiator) (cf ?cf_initiator))
	?existed_control <- (task (type Control) (cf ?cf_control))
=>
	(modify ?existed_winrate (value (cmb (cmb ?cf_push (cmb ?cf_burst (cmb ?cf_tank (cmb ?cf_initiator ?cf_control)))) -0.4)))
	(assert (sendmessagehalt (str-cat "С командой из 2-ух героев я даю вам winrate(в %):" (* (cmb (cmb ?cf_push (cmb ?cf_burst (cmb ?cf_tank (cmb ?cf_initiator ?cf_control)))) -0.4) 100))))
)

(defrule rule13
	(declare (salience 1))
	?existed_heroes <- (countHeroes (value 3))
	?existed_winrate <- (winrate (value ?wr))
	?existed_push <- (task (type Push) (cf ?cf_push))
	?existed_burst <- (task (type Burst) (cf ?cf_burst))
	?existed_tank <- (task (type Tank) (cf ?cf_tank))
	?existed_initiator <- (task (type Initiator) (cf ?cf_initiator))
	?existed_control <- (task (type Control) (cf ?cf_control))
=>
	(modify ?existed_winrate (value (cmb (cmb ?cf_push (cmb ?cf_burst (cmb ?cf_tank (cmb ?cf_initiator ?cf_control)))) 0.1)))
	(assert (sendmessagehalt (str-cat "С командой из 3-х героев я даю вам winrate(в %):" (* (cmb (cmb ?cf_push (cmb ?cf_burst (cmb ?cf_tank (cmb ?cf_initiator ?cf_control)))) 0.1) 100))))
)

(defrule rule14
	(declare (salience 1))
	?existed_heroes <- (countHeroes (value 4))
	?existed_winrate <- (winrate (value ?wr))
	?existed_push <- (task (type Push) (cf ?cf_push))
	?existed_burst <- (task (type Burst) (cf ?cf_burst))
	?existed_tank <- (task (type Tank) (cf ?cf_tank))
	?existed_initiator <- (task (type Initiator) (cf ?cf_initiator))
	?existed_control <- (task (type Control) (cf ?cf_control))
=>
	(modify ?existed_winrate (value (cmb (cmb ?cf_push (cmb ?cf_burst (cmb ?cf_tank (cmb ?cf_initiator ?cf_control)))) 0.6)))
	(assert (sendmessagehalt (str-cat "С командой из 4-ух героев я даю вам winrate(в %):" (* (cmb (cmb ?cf_push (cmb ?cf_burst (cmb ?cf_tank (cmb ?cf_initiator ?cf_control)))) 0.6) 100))))
)

(defrule rule15
	(declare (salience 1))
	?existed_heroes <- (countHeroes (value 5))
	?existed_winrate <- (winrate (value ?wr))
	?existed_push <- (task (type Push) (cf ?cf_push))
	?existed_burst <- (task (type Burst) (cf ?cf_burst))
	?existed_tank <- (task (type Tank) (cf ?cf_tank))
	?existed_initiator <- (task (type Initiator) (cf ?cf_initiator))
	?existed_control <- (task (type Control) (cf ?cf_control))
=>
	(modify ?existed_winrate (value (cmb (cmb ?cf_push (cmb ?cf_burst (cmb ?cf_tank (cmb ?cf_initiator ?cf_control)))) 0.95)))
	(assert (sendmessagehalt (str-cat "С командой из 5-х героев я даю вам winrate(в %):" (* (cmb (cmb ?cf_push (cmb ?cf_burst (cmb ?cf_tank (cmb ?cf_initiator ?cf_control)))) 0.95) 100))))
)