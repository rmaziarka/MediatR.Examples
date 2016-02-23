/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var
    gulp = require('gulp'),
    $ = require('gulp-load-plugins')({ lazy: true }),
    config = require('./gulp.config')(),
    tsProject = $.typescript.createProject(
                            {
                                "compilerOptions": {
                                    "target": "es5",
                                    "noImplicitAny": true,
                                    "sourceMap": false,
                                    "noExternalResolve": true
                                }
                            }),
    runSequence = require('run-sequence');


/**
*  Processess javascript files and adds dependency injection annotations ($inject) where /* @ngInject */
/* is found or for angular standard injections, ex controllers
*  @return {Stream}
*/
gulp.task('ts-annotate', ['ts-compile'], function () {
    return gulp.src(config.ts.outputAllJs)
		.pipe($.ngAnnotate())
		.pipe(gulp.dest(config.ts.output));
});

/**
 * Lint all typescript files
 * @return {Stream}
 */
gulp.task('ts-lint', function () {    
    return gulp
        .src(config.ts.allTs)
        .pipe($.tslint())
        .pipe($.tslint.report('prose'));
});

/**
 * Compiling Typescript --> Javascript
 * @return {Stream}
 */
gulp.task('ts-compile', function () {    
    var tsResult = gulp
        .src([config.ts.allTs, config.ts.libTypingsAllTs])
        .pipe($.sourcemaps.init())
        .pipe($.typescript(tsProject));

    tsResult.dts.pipe(gulp.dest('.'));

    return tsResult.js.pipe($.sourcemaps.write('.'))
                      .pipe(gulp.dest(config.ts.output));
});

/**
* Removes all *.js and *.js.map files from ./app
* @return {Stream}
*/
gulp.task('ts-clean', function () {
    return gulp
        .src([config.ts.outputAllJs, config.ts.outputAllJsMap])
        .pipe($.clean());
});

/**
*  Lints and compiles typescript files
*  @return {Stream}
*/
gulp.task('ts', function (callback) {
    log('TypeScript --> JavaScript ...');
    runSequence('ts-lint', 'ts-compile', 'ts-annotate', callback);    
});

/**
 * Watching changes in typescript files
 */
gulp.task('ts-watch', function () {
    gulp.watch([config.ts.allTs], ['ts']);
});

gulp.task('default', function () {
    // place code for your default task here
});

/**
 * Log a message or series of messages using chalk's blue color.
 * Can pass in a string, object or array.
 */
function log(msg) {
    if (typeof (msg) === 'object') {
        for (var item in msg) {
            if (msg.hasOwnProperty(item)) {
                $.util.log($.util.colors.blue(msg[item]));
            }
        }
    } else {
        $.util.log($.util.colors.blue(msg));
    }
}