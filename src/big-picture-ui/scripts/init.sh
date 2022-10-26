#!/bin/bash

node="v12.4.0"
clean=false

RED=$'\033[0;31m'
GREEN=$'\033[0;32m'
YELLOW=$'\033[0;33m'

while getopts ":n:c" option; do
    case $option in
    n) #set target node version that will be installed bu nvm
        node=$OPTARG ;;
    c) #clean npm cache
        clean=true ;;
    \?) # Invalid option
        echo "Error: Invalid option"
        exit
        ;;
    esac
done

cache_clean() {
    echo "$YELLOW cache cleaning..."
    npm cache clean --force >/dev/null 2>&1
    echo "$GREEN cache cleaned!"
}

node_packages_install() {
    echo "$YELLOW node packages installing..."
    npm i 2>&1
    echo "$GREEN node packages installed!"
}

start() {
    echo "$YELLOW starting..."
    npm start
}

node_modules_check() {

    if [ ../package-lock.json ]; then
        echo "$YELLOW package-lock removing..."
        rm -r ../package-lock.json >/dev/null 2>&1
        echo "$GREEN package-lock packages removed!"
    fi

    if [ -d ../node_modules ]; then
        echo "$YELLOW node  packages removing..."
        rm -r ../node_modules >/dev/null 2>&1
        echo "$GREEN node packages removed!"
    fi

}

nvm_check() {
    if ! [ -n "$(nvm -v)" ]; then
        echo "$RED NVM is not found! Please, NVM-windows download: https://github.com/coreybutler/nvm-windows/releases"
        exit 1
    else
        echo "$GREEN NVM is OK!"
    fi
}

node_version_check() {
    node_version=$(node -v)

    echo "Current Node Version: $node_version "

    if [ $node_version == $node ]; then
        echo "$GREEN Node version is ok!"
    else
        echo "$YELLOW Node $node is installing..."
        nvm install $node
        echo "$YELLOW Node $node is using..."
        nvm use $node
        echo nvm current
    fi

}

init() {
    if [ $clean = true ]; then
        cache_clean
    fi
    nvm_check
    node_version_check
    node_modules_check
    node_packages_install
    start
}

init
