/* landing.js */

'use strict';

/* ============================================  HEARTBEAT  ===================================================== */

(function () {

    let heart = document.querySelector('.fa-heart');

    function Heartbeat(Something) {
        try {
            setInterval(() => {

                if (heart !== null) {
                    heart.classList.toggle('text-danger');
                }  
                else {
                    console.log('Heartbeat: NULL ERROR!');
                }

            }, 500, );
        } catch (ex) {
            console.log('Heartbeat: CATCH ERROR!');
        }
    }

    Heartbeat();

})();

/* ================================================  POLYFILL FOR 'REMOVE ELEMENT' ==================================================== */

// from:https://github.com/jserz/js_piece/blob/master/DOM/ChildNode/remove()/remove().md
(function (arr) {
    arr.forEach(function (item) {
        if (item.hasOwnProperty('remove')) {
            return;
        }
        Object.defineProperty(item, 'remove', {
            configurable: true,
            enumerable: true,
            writable: true,
            value: function remove() {
                if (this.parentNode !== null)
                    this.parentNode.removeChild(this);
            }
        });
    });
})([Element.prototype, CharacterData.prototype, DocumentType.prototype]);


/* ================================================  POLYFILL FOR 'CLASSLIST' ==================================================== */

// Sourse: https://gist.github.com/k-gun/c2ea7c49edf7b757fe9561ba37cb19ca
 (function () {
    // helpers
    var regExp = function (name) {
        return new RegExp('(^| )' + name + '( |$)');
    };
    var forEach = function (list, fn, scope) {
        for (var i = 0; i < list.length; i++) {
            fn.call(scope, list[i]);
        }
    };

    // class list object with basic methods
    function ClassList(element) {
        this.element = element;
    }

    ClassList.prototype = {
        add: function () {
            forEach(arguments, function (name) {
                if (!this.contains(name)) {
                    this.element.className += ' ' + name;
                }
            }, this);
        },
        remove: function () {
            forEach(arguments, function (name) {
                this.element.className =
                    this.element.className.replace(regExp(name), '');
            }, this);
        },
        toggle: function (name) {
            return this.contains(name)
                ? (this.remove(name), false) : (this.add(name), true);
        },
        contains: function (name) {
            return regExp(name).test(this.element.className);
        },
        // bonus..
        replace: function (oldName, newName) {
            this.remove(oldName), this.add(newName);
        }
    };

    // IE8/9, Safari
    if (!('classList' in Element.prototype)) {
        Object.defineProperty(Element.prototype, 'classList', {
            get: function () {
                return new ClassList(this);
            }
        });
    }

    // replace() support for others
    if (window.DOMTokenList && DOMTokenList.prototype.replace === null) {
        DOMTokenList.prototype.replace = ClassList.prototype.replace;
    }
})();

/* ==================================================  STICKY FOOTER ==================================================== */

// stucky footer if visible
window.onload = function (e) {

    try {

        // get footer
        let sf = document.getElementById('stickyFooter');
        if (!document.body.hasAttribute('data-sticky')) return;

        // if training view
        if (document.getElementById('idKey') != null) {
            let fr = document.getElementById('footer-remove');
            fr.remove();
        }

        // check position, if visible => sticky footer.
        var Visible = function (foot) {
            // get element positions
            var targetPosition = {
                top: window.pageYOffset + foot.getBoundingClientRect().top,
                left: window.pageXOffset + foot.getBoundingClientRect().left,
                right: window.pageXOffset + foot.getBoundingClientRect().right,
                bottom: window.pageYOffset + foot.getBoundingClientRect().bottom
            },
                // get window positions
                windowPosition = {
                    top: window.pageYOffset,
                    left: window.pageXOffset,
                    right: window.pageXOffset + document.documentElement.clientWidth,
                    bottom: window.pageYOffset + document.documentElement.clientHeight
                };

            if (targetPosition.bottom > windowPosition.top &&
                targetPosition.top < windowPosition.bottom &&
                targetPosition.right > windowPosition.left &&
                targetPosition.left < windowPosition.right) {

                foot.classList.add('fixed-bottom');
            }
        };

        Visible(sf);

    } catch (e) {
        console.error('STICKY FOOTER [<<<FAIL>>>]');
    }
    

};

/* ===============================================  FOR TOOLTIP  =================================================== */


$(function () {

    $('[data-toggle="tooltip"]').tooltip();

});

/*===========================================  FOR DISABLED BUTTONS  =============================================== */

$('#collapseOne').on('show.bs.collapse', function () {

    try {
        let btnsDisable = document.querySelectorAll('div.d-inline-block > a.disabled');

        for (var i = 0; i < btnsDisable.length; i++) {
            btnsDisable[i].classList.remove('disabled');
        }

    } catch (e) {
        console.error('FOR DISABLED BUTTONS METHOD FAIL');
    }
   
});

/*====================================================  RAYS   ================================================ */


$(function () {

    document.addEventListener('DOMContentLoaded', (event) => {

        if (!event.target.hasAttribute('data-rays')) return;

    });

    let max_particles = 1000;

    var cns = document.createElement('canvas');
    var raysBody = document.getElementById("js-rays-body");
    cns.width = $(raysBody).width();
    cns.height = $(raysBody).height();
    $(".js-rays").append(cns);

    var canvas = cns.getContext('2d');

    class Particle {
        constructor(canvas, progress) {
            let random = Math.random();
            this.progress = 0;
            this.canvas = canvas;

            this.x = ($(raysBody).width()) + (Math.random() * 200 - Math.random() * 300);
            this.y = ($(raysBody).height()) + (Math.random() * 200 - Math.random() * 300);
            this.s = Math.random() * 1;
            this.a = 0;
            this.w = $(raysBody).width();
            this.h = $(raysBody).height();
            this.radius = random > .2 ? Math.random() * 1 : Math.random() * 3;
            this.color = random > .2 ? "#fdff52" : "#feffac";
            this.radius = random > .8 ? Math.random() * 2 : this.radius;
            this.color = random > .8 ? "#feffac" : this.color;

            // this.color  = random > .1 ? "#ffae00" : "#f0ff00" // rays
            this.variantx1 = Math.random() * 300;
            this.variantx2 = Math.random() * 400;
            this.varianty1 = Math.random() * 100;
            this.varianty2 = Math.random() * 120;
        }

        render() {
            this.canvas.beginPath();
            this.canvas.arc(this.x, this.y, this.radius, 0, 2 * Math.PI);
            this.canvas.lineWidth = 2;
            this.canvas.fillStyle = this.color;
            this.canvas.fill();
            this.canvas.closePath();
        }

        move() {
            this.x += Math.cos(this.a) * this.s;
            this.y += Math.sin(this.a) * this.s;
            this.a += Math.random() * 0.8 - 0.4;

            if (this.x < 0 || this.x > this.w - this.radius) {
                return false;
            }

            if (this.y < 0 || this.y > this.h - this.radius) {
                return false;
            }
            this.render();
            this.progress++;
            return true;
        }
    }

    let particles = [];
    let init_num = popolate(max_particles);
    function popolate(num) {
        for (var i = 0; i < num; i++) {
            setTimeout(
                function () {
                    particles.push(new Particle(canvas, i));
                }.bind(this)
                , i * 20);
        }
        return particles.length;
    }

    function clear() {
        canvas.globalAlpha = 0.05;
        canvas.fillStyle = "rgba(0, 0, 200, 0.5)";
        canvas.fillStyle = "rgba(0, 0, 200, 0.5)";
        canvas.fillRect(0, 0, cns.width, cns.height);
        canvas.globalAlpha = 1;
    }

    function update() {
        particles = particles.filter(function (p) {
            return p.move();
        });
        if (particles.length < init_num) {
            popolate(1);
        }
        clear();
        requestAnimationFrame(update.bind(this));
    }
    update();

});

/* ============================================================================================================ */


