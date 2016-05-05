module.exports = function() {
    var root = './';
    var clientApp = root + 'app/';
    var build = './build/';
    var specRunnerFile = 'specRunner.html';
    var translations = 'translations/';
    var bower = {
        json: require('./bower.json'),
        directory: './libs/',
        ignorePath: ''
    };
    var styles = root + 'styles/';

    var config = {
        index: 'index.html',
        specRunner: root + specRunnerFile,
        bower: bower,
        root: root,
        app: clientApp,
        js: {
            all: clientApp + '**/*.js',
            maps: clientApp + '**/*.js.map',
            appFiles: [
                clientApp + '**/*.js',
                '!' + clientApp + '**/*.spec.js',
                '!' + clientApp + '**/*.mock.js',
                '!' + clientApp + '**/*.test.js'
            ],
            appFilesToTest: [
                clientApp + '**/*.js',
                '!' + clientApp + '**/*.spec.js',
                '!' + clientApp + '**/*.mock.js',
                '!' + clientApp + '**/*.test.js',
                '!' + clientApp + '**/*.route.js',
                '!' + clientApp + '**/*.config.js'
            ],
            specsAndMocks: [
                clientApp + '**/*.mock.js',
                clientApp + '**/*.test.js',
                clientApp + '**/*.spec.js'
            ],
            specsAndMocksOrder: [
                '**/testHelpers/**/*',
                '**/common/**/*.mock.js',
                '**/common/**/*.test.js',
                '**/common/**/*.js',
                '**/*.mock.js',
                '**/*.test.js',
                '**/*.js'
            ],
            order: [
                '**/app.bootstrap.js',
                '**/app.module.js',
                '**/*.module.js',
                '**/common/models/business/**/*.js',
                '**/common/models/dto/**/*.js',
                '**/common/**/*.js',
                '**/*.js'
            ]
        },
        styles: {
            output: styles,
            sass: styles + '**/*.scss',
            css: styles + '*.css'
        },
        sassPaths: [
            bower.directory + 'bootstrap-sass/assets/stylesheets',
            bower.directory + 'font-awesome/scss',
            bower.directory + 'css-hamburgers/_sass/hamburgers',
        ],
        fonts: bower.directory + 'font-awesome/fonts/**/*.*',
        ts: {
            specTs: clientApp + '**/*.spec.ts',
            mockTs: clientApp + '**/*.mock.ts',
            allDtsFilePath: clientApp + 'typings/_all.d.ts',
            allTs: clientApp + '**/*.ts',
            libTypingsAllTs: root + 'typings/main/**/*.ts'
        },
        build: {
            output: build,
            temp: './.tmp/',
            testInput: './.test/',
            templateCache: {
                file: 'templates.js',
                options: {
                    module: 'app',
                    standAlone: false,
                    root: 'app/'
                }
            },
            htmlTemplatesSrc: clientApp + '**/*.html',
            fonts: bower.directory + 'font-awesome/fonts/**/*.*',
            bootstrapFonts: bower.directory + 'bootstrap-sass/assets/fonts/bootstrap/**/*.*',
            translationsDest: build + translations,
            staticTranslationsFiles: [
                translations + '*.json', // translation
            ],
            staticAppFiles: [
                root + 'wwwroot.nuspec',
                root + 'app.json',  // tokens
            ],
            webConfigFile: [
                root + '_web.config'
            ],
        },
        vendors: []
    }

    /**
     * wiredep and bower settings
     */
    config.getWiredepDefaultOptions = function() {
        var options = {
            bowerJson: config.bower.json,
            directory: config.bower.directory,
            ignorePath: config.bower.ignorePath,
            fileTypes: {
                html: {
                    replace: {
                        js: function(filePath) {
                            return '<script src="' + filePath.replace('../libs','libs') + '"></script>';
                        }
                    }
                }
            }
        };

        return options;
    };

    return config;
}
