# CMakeList.txt: проект CMake для laba1; включите исходный код и определения,
# укажите здесь логику для конкретного проекта.
#

# Добавьте источник в исполняемый файл этого проекта.
add_executable (laba3 "laba3.cpp" "bit_board.cpp" "bit_board.h" )

if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET laba3 PROPERTY CXX_STANDARD 20)
endif()

# TODO: Добавьте тесты и целевые объекты, если это необходимо.
