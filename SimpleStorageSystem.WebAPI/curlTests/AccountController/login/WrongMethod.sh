#! /bin/sh

curl -s -X GET http://localhost:5144/api/login
exec bash
