var
    gulp = require('gulp'),
    $ = require('gulp-load-plugins')({ lazy: true }),
    config = require('./gulp.config')(),
    tsProject = $.typescript.createProject({
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
 * Inject all the spec files into the specRunner.html
 * @return {Stream}
 */
gulp.task('build-specs', function (done) {
    log('building the spec runner');

    var wiredep = require('wiredep').stream;
    var options = config.getWiredepDefaultOptions();

    options.devDependencies = true;

    return gulp
        .src(config.specRunner)
        .pipe(wiredep(options))
        .pipe(inject(config.jsTest, '', config.jsOrder))
        .pipe(inject(config.specsAndMocks, 'specs', ['**/*']))
        .pipe($.inject(gulp.src(''), {
            starttag: '<!-- inject:vendors -->',
            transform: function () {
                return config.vendors.map(function (v) {
                    return '<script src="' + v + '"></script>';
                }).join('\r\n');
            }
        }))
        .pipe(gulp.dest(config.root));
});


/**
 * Inject files in a sorted sequence at a specified inject label
 * @param   {Array} src   glob pattern for source files
 * @param   {String} label   The label name
 * @param   {Array} order   glob pattern for sort order of the files
 * @returns {Stream}   The stream
 */
function inject(src, label, order, addPrefix) {
    var options = { read: false, addRootSlash: false };
    if (label) {
        options.name = 'inject:' + label;
    }
    if (addPrefix) {
        options.addPrefix = addPrefix;
    }

    return $.inject(orderSrc(src, order), options);
}

/**
 * Order a stream
 * @param   {Stream} src   The gulp.src stream
 * @param   {Array} order Glob array pattern
 * @returns {Stream} The ordered stream
 */
function orderSrc(src, order) {
    //order = order || ['**/*'];
    return gulp
        .src(src)
        .pipe($.if(order, $.order(order)));
}

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