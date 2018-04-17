/// <binding ProjectOpened='watch:all' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

/* =============================================  set paths  ============================================== */

'use strict';

const gulp = require('gulp'),
      del = require('del'),
      cssmin = require('gulp-cssmin'),
      rename = require('gulp-rename'),
      uglify = require('gulp-uglifyes'),
      merge = require('merge-stream'),
      //autoprefixer = require('gulp-autoprefixer'), 
      sass = require('gulp-sass'),
      plumber = require('gulp-plumber');


var paths = {
    webroot: './wwwroot/'
};

paths.js = paths.webroot + 'js/**/*.js';
paths.minJs = paths.webroot + 'js/**/*.min.js';
paths.css = paths.webroot + 'css/**/*.css';
paths.minCss = paths.webroot + 'css/**/*.min.css';
paths.justCss = paths.webroot + 'css';

/* ==============================================  set libs  ============================================= */

var deps = {
    'jquery': {
        'dist/*': ''
    },
    'bootstrap': {
        'dist/**/*': ''
    },
    'font-awesome': {
        'css/*': 'css',
        'fonts/*': 'fonts'
    },
    'tether': {
        'dist/js/*js': ''
    },
    'jquery-validation': {
        'dist/*': ''
    },
    'jquery-validation-unobtrusive': {
        '*.js': ''
    }
};

gulp.task('scripts', function () {

    var streams = [];

    for (var prop in deps) {
        console.log('Prepping Scripts for: ' + prop);
        for (var itemProp in deps[prop]) {
            streams.push(gulp.src('node_modules/' + prop + '/' + itemProp)
                .pipe(gulp.dest('wwwroot/lib/' + prop + '/' + deps[prop][itemProp])));
        }
    }

    return merge(streams);

});

/* ==============================================  tasks  ================================================ */

/* clean */
gulp.task('clean:js', function () {
    del.sync(paths.js);
});

gulp.task('clean:css', function () {
    del.sync(paths.css);
});

gulp.task('clean', ['clean:js', 'clean:css']);

/* min js */
gulp.task('js', function () {
    return gulp.src([paths.js, '!' + paths.minJs], { base: '.' }) 
        .pipe(plumber())
        .pipe(rename({ suffix: '.min' }))
        .pipe(uglify())
        .pipe(gulp.dest('.'));
});

/* css */
gulp.task('css', function () {
    return gulp.src('Styles/**/*.scss')
        .pipe(plumber())
        .pipe(sass())
        //.pipe(autoprefixer({
        //    browsers: ['last 3 versions'],
        //    cascade: false
        //}))

        // Original
        .pipe(gulp.dest(paths.justCss))

        // Minified
        .pipe(cssmin())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(paths.justCss));
});


/* Run task: */
gulp.task('min:all', ['js', 'css'/*, 'scripts'*/]);


/* watch: */
gulp.task('watch:all', function () {
    gulp.watch('./Styles/*.scss', ['css']);
    gulp.watch('./wwwroot/js/**/*', ['js']);
});