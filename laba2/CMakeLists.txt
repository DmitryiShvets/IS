# CMakeList.txt: проект CMake для laba1; включите исходный код и определения,
# укажите здесь логику для конкретного проекта.
#

# Добавьте источник в исполняемый файл этого проекта.
add_executable (laba2 "laba2.cpp" "bit_tag.h" "bit_tag.cpp")

if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET laba2 PROPERTY CXX_STANDARD 20)
endif()

# TODO: Добавьте тесты и целевые объекты, если это необходимо.
