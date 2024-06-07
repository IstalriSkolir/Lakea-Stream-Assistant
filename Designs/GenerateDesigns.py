import pathlib, os
import importlib.util

THEME = "DARKTHEME"
DIRECTORY = './DesignResources'

def start():
    remove_current_design_files()
    for pyfile in pathlib.Path(DIRECTORY).glob('*.py'): # or perhaps 'script*.py'
        spec = importlib.util.spec_from_file_location(f"{__name__}.imported_{pyfile.stem}" , pyfile)
        design = importlib.util.module_from_spec(spec)
        spec.loader.exec_module(design)
        design_name = generate_name(pyfile.stem)
        design.render(design_name)
        os.rename(f"{pyfile.stem}.png", f"{design_name}.png")

def remove_current_design_files():
    for file in os.listdir(os.curdir):
        if file.endswith(".png"):
            os.remove(file)

def generate_name(file_name: str) -> str:
    design_name = file_name.replace("_", " ")
    design_name = design_name.title()
    return design_name



if __name__ == "__main__":
    start()
