class CoursesController < ApplicationController
  def index
    @model = Course.all
  end

  def show
    @model = Course.find(params[:id])
  end

  def new
    @model = Course.new
    @course_cats = CourseCategory.select('id, title, short_name')
  end

  def create
    @course = Course.new(course_params)

    if @course.save
      redirect_to @course
    else
      render 'new'
    end
  end

  def edit
    @model = Course.find(params[:id])
  end

  def update

  end

  private

  def course_params
    params.require(:course).permit(:id, :title, :short_name, :description, :course_category_id)
  end
end